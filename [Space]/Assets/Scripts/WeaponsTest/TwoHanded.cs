using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class TwoHanded : MonoBehaviour
    {
        // Interaction components
        public GameObject grip;
        public GameObject trigger;

        // Interactable objects
        private NVRInteractableItem gripInt;
        private NVRInteractableItem triggerInt;

        // Rigid bodies
        private Rigidbody gripRB;
        private Rigidbody triggerRB;
        private Rigidbody gunRB;

        // Attached hands
        private NVRHand gripHand;
        private NVRHand triggerHand;

        // Interaction mode boolean
        private bool twoHands;

        private void Start()
        {
            gripInt = grip.GetComponent<NVRInteractableItem>();
            triggerInt = trigger.GetComponent<NVRInteractableItem>();
            gripRB = grip.GetComponent<Rigidbody>();
            triggerRB = trigger.GetComponent<Rigidbody>();
            gunRB = GetComponent<Rigidbody>();

            gripInt.Rigidbody = gunRB;
            triggerInt.Rigidbody = gunRB;

            twoHands = false;
        }

        private void Update()
        {
            if (twoHands)
            {
                Vector3 handVector = Vector3.Normalize(gripHand.transform.position - triggerHand.transform.position);
                Quaternion rotationDelta = Quaternion.LookRotation(handVector, Vector3.up) * Quaternion.AngleAxis(triggerHand.transform.eulerAngles.z, Vector3.forward)*Quaternion.Inverse(transform.rotation);
                Vector3 positionDelta = triggerHand.transform.position - transform.position;

                float angle;
                Vector3 axis;

                rotationDelta.ToAngleAxis(out angle, out axis);

                if (angle > 180)
                    angle -= 360;

                if (angle != 0)
                {
                    Vector3 angularTarget = angle * axis;
                    if (float.IsNaN(angularTarget.x) == false)
                    {
                        angularTarget = (angularTarget * 50f / (Time.deltaTime / NVRPlayer.NewtonVRExpectedDeltaTime)) * Time.deltaTime;
                        gunRB.angularVelocity = Vector3.MoveTowards(gunRB.angularVelocity, angularTarget, 20f);
                    }
                }

                Vector3 velocityTarget = (positionDelta * 6000f) * Time.deltaTime;
                if (float.IsNaN(velocityTarget.x) == false)
                {
                    gunRB.velocity = Vector3.MoveTowards(gunRB.velocity, velocityTarget, 10f);
                } 
            }
        }

        public virtual void modeController()
        {
            if (gripInt.AttachedHand != null)
                gripHand = gripInt.AttachedHand;
            if (triggerInt.AttachedHand != null)
                triggerHand = triggerInt.AttachedHand;

            if (gripHand != null && triggerHand != null)
            {
                twoHands = true;
                gripInt.Rigidbody = gripRB;
                triggerInt.Rigidbody = triggerRB;
                gunRB.useGravity = false;
            }
        }

        public virtual void gripRelease()
        {
            if (gripInt.AttachedHand == null)
                gripHand = null;
            if (triggerInt.AttachedHand == null)
                triggerHand = null;

            if (twoHands == true)
            {
                twoHands = false;
                gripInt.Rigidbody = gunRB;
                triggerInt.Rigidbody = gunRB;
            }
        }
    }
}
