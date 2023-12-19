////////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2021 zSpace, Inc.  All Rights Reserved.
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;

using UnityEngine;

using zSpace.Core.Extensions;
using zSpace.Core.Interop;
using zSpace.Core.Sdk;

namespace zSpace.Core
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(ScriptPriority)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed partial class ZCamera : MonoBehaviour
    {
        public const int ScriptPriority = ZProvider.ScriptPriority + 20;

        public enum RenderMode
        {
            SingleCamera = 0,
            MultiCamera = 1,
        }

        ////////////////////////////////////////////////////////////////////////
        // Inspector Fields
        ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Flag to control whether stereoscopic 3D rendering is enabled.
        /// </summary>
        [Tooltip(
            "Flag to control whether stereoscopic 3D rendering is enabled.")]
        public bool EnableStereo = true;

        /// <summary>
        /// The time in seconds to wait while the head target is not visible 
        /// before initiating the automatic transition from stereoscopic 3D 
        /// to monoscopic 3D rendering.
        /// </summary>
        [Tooltip(
            "The time in seconds to wait while the head target is not " +
            "visible before initiating the automatic transition from " +
            "stereoscopic 3D to monoscopic 3D rendering.")]
        public float StereoToMonoDelay = 5.0f;

        /// <summary>
        /// The duration in seconds of the transition from stereoscopic 3D
        /// to monoscopic 3D rendering (and vice versa).
        /// </summary>
        [Tooltip(
            "The duration in seconds of the transition from stereoscopic 3D " +
            "to monoscopic 3D rendering (and vice versa).")]
        public float StereoToMonoDuration = 1.0f;

        /// <summary>
        /// The camera's stereoscopic 3D render mode.
        /// </summary>
        /// 
        /// <remarks>
        /// SingleCamera (default) and MultiCamera are the two currently 
        /// supported stereoscopic 3D render modes. 
        /// 
        /// The SingleCamera render mode is more optimal due to it being able
        /// to share culling and shadow passes for both the left and right
        /// eyes. As a result, there are noticable visual artifacts when 
        /// rendering features such as shadows.
        /// 
        /// If your application requires shadows, please use the MultiCamera
        /// render mode to avoid these visual artifacts. Note, that if the 
        /// MultiCamera render mode is enabled, any post-process rendering
        /// related camera scripts must be added to the secondary left and right
        /// child camera GameObjects.
        /// </remarks>
        [Tooltip("The camera's stereoscopic 3D render mode.")]
        public RenderMode StereoRenderMode = RenderMode.SingleCamera;

        /// <summary>
        /// The left eye camera to be used when StereoRenderMode is set to
        /// RenderMode.MultiCamera.
        /// </summary>
        [Tooltip(
            "The left eye camera to be used when StereoRenderMode is set to " +
            "RenderMode.MultiCamera.")]
        [SerializeField]
        private Camera _leftCamera = null;

        /// <summary>
        /// The right eye camera to be used when StereoRenderMode is set to
        /// RenderMode.MultiCamera.
        /// </summary>
        [Tooltip(
            "The right eye camera to be used when StereoRenderMode is set to " +
            "RenderMode.MultiCamera.")]
        [SerializeField]
        private Camera _rightCamera = null;

        ////////////////////////////////////////////////////////////////////////
        // MonoBehaviour Callbacks
        ////////////////////////////////////////////////////////////////////////

        private void Reset()
        {
            Camera camera = this.Camera;

            camera.stereoSeparation = ZFrustum.DefaultIpd;
            camera.nearClipPlane = ZFrustum.DefaultNearClip;
            camera.farClipPlane = ZFrustum.DefaultFarClip;
        }

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.EndOfFrameUpdate());
            }
        }

        private void Awake()
        {
            this._camera = this.GetComponent<Camera>();

            if (Application.isPlaying)
            {
#if UNITY_EDITOR
                if (!this.CompareTag("MainCamera"))
                {
                    Debug.LogWarningFormat(
                        this,
                        "<color=cyan>{0}</color> will not render to the XR " +
                        "Overlay. To enable XR Overlay rendering, please set " +
                        "{0}'s associated tag to \"MainCamera\".",
                        this.name);
                }
#endif

                if (ZProvider.IsInitialized)
                {
                    this._context = ZProvider.Context;
                    this._headTarget = ZProvider.HeadTarget;
                    this._leftEyeTarget = ZProvider.LeftEyeTarget;
                    this._rightEyeTarget = ZProvider.RightEyeTarget;
                    this._frustum = ZProvider.Viewport.Frustum;

                    this._contextStereoDisplayMode =
                        this._context.StereoDisplayMode;

                    if (this._contextStereoDisplayMode ==
                        ZStereoDisplayMode.NativePlugin)
                    {
                        this.InitializeNativePluginStereoDisplay();
                    }
                }

                // Initialize members related to transitioning from stereo
                // to mono (and vice versa).
                bool isHeadVisible = false;

                if (this._headTarget != null)
                {
                    isHeadVisible = this._headTarget.IsVisible;
                }

                this._stereoWeight = isHeadVisible ? 1 : 0;
                this._stereoTimeRemaining = this.StereoToMonoDelay;

                // Initialize the internal early updater.
                this._earlyUpdater =
                    this.gameObject.AddComponent<EarlyUpdater>();
                this._earlyUpdater.Camera = this;
                this._earlyUpdater.hideFlags = HideFlags.HideInInspector;

                // Initialize the internal late updater.
                this._lateUpdater = this.gameObject.AddComponent<LateUpdater>();
                this._lateUpdater.Camera = this;
                this._lateUpdater.hideFlags = HideFlags.HideInInspector;
            }
        }

        private void Update()
        {
            this.UpdateTransform();

            this.UpdateStereoWeight();

            this.UpdatePerspective();

            // The camera target textures must be updated after updating the
            // camera perspectives because updating the perspectives overwrites
            // the target textures for some cameras (specifically the left and
            // right cameras, which have their properties overwritten with
            // properties copied from the main camera).
            this.UpdateTargetTexture();
        }

        private void OnApplicationPause(bool isPaused)
        {
            // Disable stereoscopic 3D rendering if the application is paused.
            if (isPaused)
            {
                this._stereoWeight = 0;
            }
        }

        private void OnDestroy()
        {
            if (this._earlyUpdater != null)
            {
                Destroy(this._earlyUpdater);
            }

            if (this._lateUpdater != null)
            {
                Destroy(this._lateUpdater);
            }

#if UNITY_EDITOR
            this.DestroyOverlayResources();
#endif
        }

        ////////////////////////////////////////////////////////////////////////
        // Public Properties
        ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the associated Unity Camera.
        /// </summary>
        public Camera Camera
        {
            get
            {
                return this._camera;
            }
        }

        /// <summary>
        /// The current scale of the world.
        /// </summary>
        /// 
        /// <remarks>
        /// The world scale is computed as the product of the parent camera
        /// rig's viewer scale multiplied by the current display scale factor
        /// accessible via ZProvider.DisplayScaleFactor.
        /// </remarks>
        public Vector3 WorldScale
        {
            get
            {
                return this._worldScale;
            }
        }

        /// <summary>
        /// Gets the camera's offset in meters.
        /// </summary>
        public Vector3 CameraOffset
        {
            get
            {
                return this._cameraOffset;
            }
        }

        /// <summary>
        /// The transformation matrix from camera to world space.
        /// </summary>
        /// 
        /// <remarks>
        /// This is useful in scenarios such as transforming a 6-DOF
        /// trackable target's pose from camera space to world space.
        /// </remarks>
        public Matrix4x4 CameraToWorldMatrix
        {
            get
            {
                return this._monoLocalToWorldMatrix;
            }
        }

        /// <summary>
        /// The world space transformation matrix of the zero parallax 
        /// (screen) plane.
        /// </summary>
        public Matrix4x4 ZeroParallaxLocalToWorldMatrix
        {
            get
            {
                if (this.transform.parent != null)
                {
                    return this.transform.parent.localToWorldMatrix;
                }
                else
                {
                    return Matrix4x4.identity;
                }
            }
        }

        /// <summary>
        /// The world space pose of the zero parallax (screen) plane.
        /// </summary>
        public Pose ZeroParallaxPose
        {
            get
            {
                if (this.transform.parent != null)
                {
                    return this.transform.parent.ToPose();
                }
                else
                {
                    return new Pose(Vector3.zero, Quaternion.identity);
                }
            }
        }

        /// <summary>
        /// The Unity Plane in world space representing the zero parallax 
        /// (screen) plane.
        /// </summary>
        public Plane ZeroParallaxPlane
        {
            get
            {
                if (this.transform.parent != null)
                {
                    return new Plane(-this.transform.parent.forward, this.transform.parent.position);
                }
                else
                {
                    return new Plane(Vector3.back, Vector3.zero);
                }
            }
        }

        /// <summary>
        /// Gets whether stereoscopic 3D rendering capabilities are available.
        /// </summary>
        public bool IsStereoAvailable
        {
            get
            {
                return (this._frustum != null);
            }
        }
        
        /// <summary>
        /// The current weight value between 0 and 1 (inclusive) that 
        /// represents whether the camera's perspective is monoscopic
        /// or stereoscopic 3D.
        /// </summary>
        /// 
        /// <remarks>
        /// The only time this value will be in between 0 and 1 is when the
        /// camera is performing a transition from stereoscopic to monoscopic
        /// 3D (or vice versa).
        /// 
        /// Additionally, a value of 0 means the camera is rendering a 
        /// monoscopic 3D perspective. A value of 1 means the camera is 
        /// rendering a stereoscopic 3D perspective.
        /// </remarks>
        public float StereoWeight
        {
            get
            {
                return this._stereoWeight;
            }
        }

        ////////////////////////////////////////////////////////////////////////
        // Private Methods
        ////////////////////////////////////////////////////////////////////////

        private void InitializeNativePluginStereoDisplay()
        {
            // Issue an event to the native plugin to tell it to enable the
            // native plugin context's graphics API binding on the Unity render
            // thread.
            ZPlugin.IssueEvent(
                ZPluginEvent.EnableNativePluginContextGraphicsBinding);

            // Create the render textures that will contain the per-eye images
            // to be displayed by the native plugin.

            var perEyeImageResolution = this._context.PerEyeImageResolution;

            this._nativePluginLeftEyeRenderTexture = new RenderTexture(
                width: perEyeImageResolution.x,
                height: perEyeImageResolution.y,
                depth: 24,
                format: RenderTextureFormat.ARGB32);
            this._nativePluginLeftEyeRenderTexture.Create();

            this._nativePluginRightEyeRenderTexture = new RenderTexture(
                width: perEyeImageResolution.x,
                height: perEyeImageResolution.y,
                depth: 24,
                format: RenderTextureFormat.ARGB32);
            this._nativePluginRightEyeRenderTexture.Create();

            // Assume that both eye render textures have the same sRGB setting.
            var areEyeRenderTexturesSRGB =
                this._nativePluginLeftEyeRenderTexture.sRGB;

            // Give the per-eye render textures to the native plugin to use
            // later for stereo display.
            ZPlugin.SetStereoDisplayTextures(
                this._nativePluginLeftEyeRenderTexture.GetNativeTexturePtr(),
                this._nativePluginRightEyeRenderTexture.GetNativeTexturePtr(),
                areEyeRenderTexturesSRGB);
        }

        private void BeginFrame()
        {
            if (this._context != null)
            {
                this._context.BeginFrame();
            }
        }

        private void UpdateTransform()
        {
            this._cameraOffset = (Vector3.back * ZProvider.WindowSize.magnitude);

            this.transform.localPosition = this._cameraOffset;
            this.transform.localRotation = Quaternion.identity;
            this.transform.localScale = Vector3.one;

            this._worldScale = this.transform.lossyScale;

            this._monoLocalToWorldMatrix = this.transform.localToWorldMatrix;
            this._monoWorldToCameraMatrix = this.Camera.worldToCameraMatrix;

            this._monoLocalPoseMatrix = Matrix4x4.TRS(
                this.transform.localPosition,
                this.transform.localRotation,
                Vector3.one);
        }

        private void UpdateStereoWeight()
        {
            if (this._headTarget == null)
            {
                return;
            }

            bool transitionToStereo =
                this.EnableStereo && this._headTarget.IsVisible;

            // If the native plugin stereo display mode is requested,
            // transition to mono if stereo display is disabled to produce
            // better looking mono images.
            if (this.IsStereoAvailable &&
                this._contextStereoDisplayMode ==
                    ZStereoDisplayMode.NativePlugin)
            {
                if (this._context != null &&
                    !this._context.IsStereoDisplayEnabled)
                {
                    transitionToStereo = false;
                }
            }

            float maxDelta = (this.StereoToMonoDuration != 0) ?
                Time.unscaledDeltaTime / this.StereoToMonoDuration :
                float.MaxValue;

            if (transitionToStereo)
            {
                // Start transitioning from mono to stereo immediately
                // after the head becomes visible.
                this._stereoTimeRemaining = this.StereoToMonoDelay;

                this._stereoWeight = Mathf.MoveTowards(
                    this._stereoWeight, 1, maxDelta);
            }
            else
            {
                // Start transitioning from stereo to mono after the
                // specified stereo to mono delay.
                if (this.EnableStereo && this._stereoTimeRemaining > 0)
                {
                    this._stereoTimeRemaining -= Time.unscaledDeltaTime;
                }
                else
                {
                    this._stereoWeight = Mathf.MoveTowards(
                        this._stereoWeight, 0, maxDelta);
                }

                // If the application is not in focus and does not run in the
                // background, transition from stereo to mono immediately to
                // help ensure that a mono render is performed before the
                // application stops running.
                if (!Application.isFocused && !Application.runInBackground)
                {
                    this._stereoWeight = 0;
                }
            }
        }

        private void UpdatePerspective()
        {
            // Update the main camera's perspective.
            if (!Application.isPlaying || !this.IsStereoAvailable)
            {
                this.UpdateMonoPerspective();
            }
            else
            {
                this.UpdateStereoPerspective();
            }

            // Update the left and right camera perspectives.
            this.UpdateSecondaryCameraPerspectives();
        }

        private void UpdateTargetTexture()
        {
            // If native plugin stereo display is requested, then set the left
            // and right camera target textures to the render textures that
            // will be used by the native plugin for stereo display.
            //
            // The main camera's target texture is not set here because it must
            // be set before rendering each eye since there is a different
            // target texture for each eye and the main camera only has one
            // target texture.  If the main camera is being used (i.e. if
            // single-camera mode is requested), its target texture will be set
            // immediately before it is manually rendered for each eye at the
            // end of the frame.
            if (this.IsStereoAvailable &&
                this._contextStereoDisplayMode ==
                    ZStereoDisplayMode.NativePlugin)
            {
                if (this._leftCamera != null)
                {
                    this._leftCamera.targetTexture =
                        this._nativePluginLeftEyeRenderTexture;
                }

                if (this._rightCamera != null)
                {
                    this._rightCamera.targetTexture =
                        this._nativePluginRightEyeRenderTexture;
                }
            }
        }

        private void UpdateMonoPerspective()
        {
            Camera camera = this.Camera;

            // Compute the half extents of the corresponding to the positions
            // of the left, right, top, and bottom frustum bounds.
            float nearScale = camera.nearClipPlane / this._cameraOffset.magnitude;
            Vector2 halfExtents = ZProvider.WindowSize * 0.5f * nearScale;

            // Compute and set the monoscopic projection matrix.
            Matrix4x4 projectionMatrix = Matrix4x4.Frustum(
                -halfExtents.x, halfExtents.x, -halfExtents.y, halfExtents.y,
                camera.nearClipPlane, camera.farClipPlane);

            camera.projectionMatrix = projectionMatrix;

            // Set the stereo view and projection matrices to be equal
            // to the monoscopic view and projection matrices.
            camera.SetStereoViewMatrix(
                Camera.StereoscopicEye.Left, this._monoWorldToCameraMatrix);

            camera.SetStereoViewMatrix(
                Camera.StereoscopicEye.Right, this._monoWorldToCameraMatrix);

            camera.SetStereoProjectionMatrix(
                Camera.StereoscopicEye.Left, projectionMatrix);

            camera.SetStereoProjectionMatrix(
                Camera.StereoscopicEye.Right, projectionMatrix);
        }

        private void UpdateStereoPerspective()
        {
            Camera camera = this.Camera;

            // Apply camera settings to the frustum.
            this._frustum.NearClip = camera.nearClipPlane;
            this._frustum.FarClip = camera.farClipPlane;
            this._frustum.CameraOffset = this._cameraOffset;

            this._frustum.Ipd = camera.stereoSeparation;

            var interpolatedLeftEyePose = PoseExtensions.Lerp(
                this._frustum.DefaultEyePose,
                this._leftEyeTarget.Pose,
                this._stereoWeight);

            var interpolatedRightEyePose = PoseExtensions.Lerp(
                this._frustum.DefaultEyePose,
                this._rightEyeTarget.Pose,
                this._stereoWeight);

            this._frustum.SetTrackerSpaceEyePoses(
                interpolatedLeftEyePose, interpolatedRightEyePose);

            // Update the camera's view matrices for the 
            // center, left, and right eyes.
            camera.transform.SetLocalPose(this.GetLocalPose(ZEye.Center));

            camera.SetStereoViewMatrix(
                Camera.StereoscopicEye.Left,
                this._frustum.GetViewMatrix(ZEye.Left, this.WorldScale) * 
                this._monoWorldToCameraMatrix);

            camera.SetStereoViewMatrix(
                Camera.StereoscopicEye.Right,
                this._frustum.GetViewMatrix(ZEye.Right, this.WorldScale) * 
                this._monoWorldToCameraMatrix);

            // Update the camera's projection matrices for the 
            // center, left, and right eyes.
            camera.projectionMatrix = 
                this._frustum.GetProjectionMatrix(ZEye.Center);

            camera.SetStereoProjectionMatrix(
                Camera.StereoscopicEye.Left,
                this._frustum.GetProjectionMatrix(ZEye.Left));

            camera.SetStereoProjectionMatrix(
                Camera.StereoscopicEye.Right,
                this._frustum.GetProjectionMatrix(ZEye.Right));
        }

        private void UpdateSecondaryCameraPerspectives()
        {
            Camera camera = this.Camera;

            if (this._leftCamera != null)
            {
                this._leftCamera.CopyFrom(
                    camera, Camera.StereoscopicEye.Left);

                this._leftCamera.transform.SetPose(
                    this.GetPose(ZEye.Left), true);
            }

            if (this._rightCamera != null)
            {
                this._rightCamera.CopyFrom(
                    camera, Camera.StereoscopicEye.Right);

                this._rightCamera.transform.SetPose(
                    this.GetPose(ZEye.Right), true);
            }
        }

        private void UpdateCameraActiveState()
        {
            // If stereo is not available or if Unity quad-buffer stereo
            // rendering is requested, then set the camera active state
            // according to the requested single-camera/multi-camera mode.
            if (!this.IsStereoAvailable ||
                this._contextStereoDisplayMode ==
                    ZStereoDisplayMode.UnityQuadBufferStereo)
            {
                bool isPrimaryCameraEnabled =
                    (this.StereoRenderMode == RenderMode.SingleCamera);

                // Update whether the main camera is enabled.
                this.Camera.enabled = isPrimaryCameraEnabled;

                // Update whether the secondary left and right cameras
                // are enabled.
                if (this._leftCamera != null)
                {
                    this._leftCamera.gameObject.SetActive(
                        !isPrimaryCameraEnabled);
                    this._leftCamera.enabled = !isPrimaryCameraEnabled;
                }

                if (this._rightCamera)
                {
                    this._rightCamera.gameObject.SetActive(
                        !isPrimaryCameraEnabled);
                    this._rightCamera.enabled = !isPrimaryCameraEnabled;
                }
            }
            // If native plugin stereo display is requested, always disable the
            // main camera as it will be either rendered manually at the end of
            // the frame (if single-camera mode is requested) or not used (if
            // multi-camera mode is requested). Also set the active state of
            // the left and right cameras according to the requested
            // single-camera/multi-camera mode.
            else if (this._contextStereoDisplayMode ==
                ZStereoDisplayMode.NativePlugin)
            {
                this.Camera.enabled = false;

                bool areSecondaryCamerasEnabled =
                    this.StereoRenderMode == RenderMode.MultiCamera;

                if (this._leftCamera != null)
                {
                    this._leftCamera.gameObject.SetActive(
                        areSecondaryCamerasEnabled);
                    this._leftCamera.enabled = areSecondaryCamerasEnabled;
                }

                if (this._rightCamera)
                {
                    this._rightCamera.gameObject.SetActive(
                        areSecondaryCamerasEnabled);
                    this._rightCamera.enabled = areSecondaryCamerasEnabled;
                }
            }
        }

        private Pose GetPose(ZEye eye)
        {
            if (this._frustum != null)
            {
                Matrix4x4 viewMatrix =
                    this._frustum.GetViewMatrix(eye).FlipHandedness();

                Matrix4x4 localToWorldMatrix =
                    this._monoLocalToWorldMatrix * viewMatrix.inverse;

                return localToWorldMatrix.ToPose();
            }
            else
            {
                return this._monoLocalToWorldMatrix.ToPose();
            }
        }

        private Pose GetLocalPose(ZEye eye)
        {
            if (this._frustum != null)
            {
                Matrix4x4 viewMatrix = 
                    this._frustum.GetViewMatrix(eye).FlipHandedness();

                Matrix4x4 localPoseMatrix =
                    this._monoLocalPoseMatrix * viewMatrix.inverse;

                return localPoseMatrix.ToPose();
            }

            return this._monoLocalPoseMatrix.ToPose();
        }

        private void PerformSingleCameraNativePluginStereoDisplayEyeRendering()
        {
            this.Camera.Render(
                this._nativePluginLeftEyeRenderTexture,
                Camera.StereoscopicEye.Left,
                this.GetPose(ZEye.Left));

            this.Camera.Render(
                this._nativePluginRightEyeRenderTexture,
                Camera.StereoscopicEye.Right,
                this.GetPose(ZEye.Right));
        }

        private IEnumerator EndOfFrameUpdate()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                // If native plugin stereo display is requested, perform the
                // rendering according to the requested
                // single-camera/multi-camera mode.
                if (this.IsStereoAvailable &&
                    this._contextStereoDisplayMode ==
                        ZStereoDisplayMode.NativePlugin)
                {
                    // If single-camera mode is requested, perform the rendering
                    // for each eye by manually rendering the main camera.
                    if (this.StereoRenderMode == RenderMode.SingleCamera)
                    {
                        this.PerformSingleCameraNativePluginStereoDisplayEyeRendering();
                    }

                    // If multi-camera mode is requested, the rendering for
                    // each eye will have already been performed by the left
                    // and right cameras.

                    // Regardless of the requested single-camera/multi-camera
                    // mode, issue an event to the native plugin to tell it to
                    // display the latest stereo images on the Unity render
                    // thread.
                    ZPlugin.IssueEvent(
                        ZPluginEvent.SubmitFrameToNativePluginContext);
                }

                if (this.Camera != null)
                {
                    this.Camera.enabled = true;
                }

                if (this._context != null)
                {
                    this._context.EndFrame();
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////
        // Private Types
        ////////////////////////////////////////////////////////////////////////

        // Make the default script execution order low enough to hopefully
        // ensure that the native plugin context has been notified that a new
        // frame is beginning before all MonoBehaviour Update()callbacks have
        // had a chance to run.
        [DefaultExecutionOrder(-10000)]
        private class EarlyUpdater : MonoBehaviour
        {
            public ZCamera Camera { get; set; }

            private void Update()
            {
                this.Camera.BeginFrame();
            }
        }

        // Make the default script execution order high enough to hopefully 
        // ensure that the camera active state and XR Overlay will be updated 
        // after all MonoBehaviour Update() and LateUpdate() callbacks have had 
        // a chance to run.
        [DefaultExecutionOrder(10000)]
        private class LateUpdater : MonoBehaviour
        {
            public ZCamera Camera { get; set; }

            private void LateUpdate()
            {
                if (this.Camera != null)
                {
                    // Ensure the appropriate cameras are enabled prior to 
                    // rendering.
                    this.Camera.UpdateCameraActiveState();

#if UNITY_EDITOR_WIN
                    // NOTE: Updating and rendering to the XR Overlay performs 
                    //       best when executed from MonoBehaviour.LateUpdate().
                    if (this.Camera.enabled)
                    {
                        this.Camera.UpdateOverlay();
                    }
#endif
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////
        // Private Members
        ////////////////////////////////////////////////////////////////////////

        private Camera _camera = null;

        private ZContext _context = null;
        private ZTarget _headTarget = null;
        private ZTarget _leftEyeTarget = null;
        private ZTarget _rightEyeTarget = null;
        private ZFrustum _frustum = null;

        private EarlyUpdater _earlyUpdater = null;
        private LateUpdater _lateUpdater = null;

        private ZStereoDisplayMode _contextStereoDisplayMode =
            ZStereoDisplayMode.UnityQuadBufferStereo;

        private Vector3 _cameraOffset = ZFrustum.DefaultCameraOffset;
        private Vector3 _worldScale = Vector3.one;

        private Matrix4x4 _monoLocalToWorldMatrix;
        private Matrix4x4 _monoWorldToCameraMatrix;
        private Matrix4x4 _monoLocalPoseMatrix;

        private float _stereoWeight = 1;
        private float _stereoTimeRemaining = 0;

        private RenderTexture _nativePluginLeftEyeRenderTexture;
        private RenderTexture _nativePluginRightEyeRenderTexture;
    }
}
