////////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2020 zSpace, Inc.  All Rights Reserved.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.EventSystems;

using zSpace.Core.Sdk;
using zSpace.Core.EventSystems;
using zSpace.Core.Input;

namespace zSpace.Core.Samples
{
    public class StylusVibrationFeedback1 : ZPointerInteractable,
        IPointerDownHandler , IPointerUpHandler 
    {
        ////////////////////////////////////////////////////////////////////////
        // Inspector Fields
        ////////////////////////////////////////////////////////////////////////


        public float VibrationIntensity;
        public Color HoverColor;
        bool down;
        ZStylus stylus;
        bool buttonDown_0;


        ////////////////////////////////////////////////////////////////////////
        // Enumerators
        ////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////
        // MonoBehaviour Callbacks
        ////////////////////////////////////////////////////////////////////////
        public override ZPointer.DragPolicy GetDragPolicy(ZPointer pointer)
        {
            if (pointer is ZMouse)
            {
                return ZPointer.DragPolicy.LockToScreenAlignedPlane;
            }

            if (pointer is ZStylus)
            {
                return ZPointer.DragPolicy.LockHitPosition;
            }

            return base.GetDragPolicy(pointer);
        }
        private void Start()
        {
            if (ZProvider.IsInitialized)
            {
                this._stylusTarget = ZProvider.StylusTarget;
                this._stylusTarget.IsVibrationEnabled = true;
                stylus = GameObject.FindObjectOfType<ZStylus>();
                if (ZProvider.CurrentDisplay.Size !=
                    ZDisplay.GetSize(ZDisplay.Profile.Size24InchAspect16x9))
                    {
                    Debug.LogWarning("AIO model hardware not detected.\n " +
                        "Stylus vibration and LED light feedback will not " +
                        "be experienced.");
                }
            }
            else
            {
                Debug.LogWarning("ZProvider can not initialize.\n Stylus" +
                    "vibration and LED light feedback will not be experienced.");

                Destroy(this);
            }
        }
        public void Update()
        {

            
            if (down)
            {
            this.Vibrate();
            this._stylusTarget.IsLedEnabled = true;
            this._stylusTarget.LedColor = HoverColor;
            }
            else
            {
            this._stylusTarget.StopVibration();
            this._stylusTarget.IsLedEnabled = false;
            }
        }
        ////////////////////////////////////////////////////////////////////////
        // Public Methods
        ////////////////////////////////////////////////////////////////////////

        public void OnPointerDown(PointerEventData eventData)
        {
            ZPointerEventData pointerEventData = eventData as ZPointerEventData;
            if (pointerEventData == null ||
                pointerEventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            down = true;
          
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ZPointerEventData pointerEventData = eventData as ZPointerEventData;
            if (pointerEventData == null ||
                pointerEventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            down = false;
            
            
        }   

        ////////////////////////////////////////////////////////////////////////
        // Private Methods
        ////////////////////////////////////////////////////////////////////////
        private void Vibrate()
        {
                    this._stylusTarget.StartVibration(
                        1, 2,50 , this.VibrationIntensity);
        }

        ////////////////////////////////////////////////////////////////////////
        // Private Members
        ////////////////////////////////////////////////////////////////////////

        private ZTarget _stylusTarget;
    }
}
