using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class SnappableIO : MonoBehaviour
    {
        NVRInteractableItem item;
        NVRHand hand;

        //private bool rotationAlignment = true;
        //private bool positionAlignment = true;
        private bool isSnapped = false;
        public float snapTranslationRate = 0.5f;
        public float snapRotationRate = 360.0f;

        private void Update()
        {
            if (isSnapped && item != null)
            {
                item.transform.position = this.transform.position;
                item.transform.localRotation = this.transform.rotation;

                if (item.AttachedHand != null)
                {
                    isSnapped = false;
                    item = null;
                }
            }
        }

        private void OnTriggerEnter(Collider snapBox)
        {
            if (!isSnapped)
            {
                item = snapBox.gameObject.GetComponent<NVRInteractableItem>();
                if (item != null)
                {
                    hand = item.AttachedHand;
                    if (hand != null && hand.HoldButtonPressed)
                    {
                        snapObject();
                    }
                }
            }
        }

        private void OnTriggerStay(Collider snapBox)
        {
            if (!isSnapped)
            {
                if (item != null)
                {
                    if (hand.HoldButtonPressed)
                    {
                        snapObject();
                    }
                }
            }
        }

        private void snapObject()
        {
            item.ForceDetach();
            item.AttachedHand = null;
            hand = null;

            item.GetComponent<Rigidbody>().isKinematic = true;
            item.GetComponent<Rigidbody>().useGravity = false;
            isSnapped = true;
            //rotationAlignment = false;
            //positionAlignment = false;
        }
        /*
        private void alignRotation()
        {
            if (item.gameObject.transform.localRotation != this.transform.rotation)
                item.gameObject.transform.localRotation = Quaternion.RotateTowards(item.gameObject.transform.localRotation, this.transform.rotation, Time.deltaTime * snapRotationRate);
            else
                rotationAlignment = true;
        }

        private void alignPosition()
        {
            if (item.gameObject.transform.position != this.transform.position)
                item.gameObject.transform.position = Vector3.MoveTowards(item.gameObject.transform.position, this.transform.position, Time.deltaTime * snapTranslationRate);
            else
                positionAlignment = true;
        }
        */
    }
}
