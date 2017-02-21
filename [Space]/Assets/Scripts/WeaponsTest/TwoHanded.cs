using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractableItem))]
    public class TwoHanded : MonoBehaviour
    {
        private NVRInteractableItem gun;
        public Transform gripPoint;
        private NVRHand triggerHand;
        private NVRHand gripHand;

        private bool twoHands = false;

        // Use this for initialization
        void Start()
        {
            gun = this.GetComponent<NVRInteractableItem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (twoHands)
            {

            }
        }

        private void OnTriggerEnter(Collider detectHand)
        {
            if (gun.AttachedHand != null)
            {
                triggerHand = gun.AttachedHand;
                gripHand = detectHand.gameObject.GetComponent<NVRHand>();
                if (gripHand != null && gripHand.CurrentlyInteracting == null && gripHand.HoldButtonDown)
                {
                    twoHands = true;
                }
            }
        }
    }
}
