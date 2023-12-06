////////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 2007-2020 zSpace, Inc.  All Rights Reserved.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.EventSystems;

using zSpace.Core.Sdk;

namespace zSpace.Core.Samples
{
    public class StylusVibrationFeedback1 : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler
    {
        ////////////////////////////////////////////////////////////////////////
        // Inspector Fields
        ////////////////////////////////////////////////////////////////////////

        public VibrationTypeEnum VibrationType;
        public float VibrationIntensity;

        ////////////////////////////////////////////////////////////////////////
        // Enumerators
        ////////////////////////////////////////////////////////////////////////

        public enum VibrationTypeEnum
        {
            Constant,
            FastPulse,
            MediumPulse,
            SlowPulse
        }

        ////////////////////////////////////////////////////////////////////////
        // MonoBehaviour Callbacks
        ////////////////////////////////////////////////////////////////////////

        private void Start()
        {
            if (ZProvider.IsInitialized)
            {
                this._stylusTarget = ZProvider.StylusTarget;
                this._stylusTarget.IsVibrationEnabled = true;
            }
            else
            {
                Debug.LogWarning("ZProvider can not initialize.\n Stylus" +
                    "vibration and LED light feedback will not be experienced.");

                Destroy(this);
            }
        }

        ////////////////////////////////////////////////////////////////////////
        // Public Methods
        ////////////////////////////////////////////////////////////////////////

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.Vibrate();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this._stylusTarget.StopVibration();
        }

        ////////////////////////////////////////////////////////////////////////
        // Private Methods
        ////////////////////////////////////////////////////////////////////////

        private void Vibrate()
        {
            switch (this.VibrationType)
            {
                case StylusVibrationFeedback1.VibrationTypeEnum.Constant:
                    this._stylusTarget.StartVibration(
                        1.0f, 0.0f, 100, this.VibrationIntensity);
                    break;

                case StylusVibrationFeedback1.VibrationTypeEnum.FastPulse:
                    this._stylusTarget.StartVibration(
                        0.1f, 0.1f, 100, this.VibrationIntensity);
                    break;

                case StylusVibrationFeedback1.VibrationTypeEnum.MediumPulse:
                    this._stylusTarget.StartVibration(
                        0.3f, 0.3f, 100, this.VibrationIntensity);
                    break;

                case StylusVibrationFeedback1.VibrationTypeEnum.SlowPulse:
                    this._stylusTarget.StartVibration(
                        0.6f, 0.6f, 100, this.VibrationIntensity);
                    break;

                default:
                    break;
            }
        }

        ////////////////////////////////////////////////////////////////////////
        // Private Members
        ////////////////////////////////////////////////////////////////////////

        private ZTarget _stylusTarget;
    }
}
