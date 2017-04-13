using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space {
    public class HapticController : MonoBehaviour {

        private NVRInteractable oneHanded;
        private TwoHandedInteractable twoHanded;

        private ushort hapticStrength;

        private bool twoHands;

        void Start()
        {
            twoHanded = GetComponent<TwoHandedInteractable>();
            if (twoHanded != null)
                twoHands = true;
            else
            {
                oneHanded = GetComponent<NVRInteractable>();
                twoHands = false;
            }
        }

        public void pulse()
        {
            if (twoHanded)
            {
                twoHanded.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
                if (twoHanded.SecondAttachedHand != null)
                    twoHanded.SecondAttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
            }
            else
                oneHanded.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
        }

        public void setParams(ushort hapticStrengthIn)
        {
            hapticStrength = hapticStrengthIn;
        }
    }
}
