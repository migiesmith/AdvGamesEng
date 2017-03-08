using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Reloadable : MonoBehaviour
    {
        private Transform magwell;

        public int ammoCapacity;
        public float ammoCount;
        public string magName;
        private GameObject magazine;
        private Rigidbody magRB;
        private NVRInteractableItem magInt;
        private Collider magCol;

        // Use this for initialization
        void Start()
        {
            magwell = transform.FindChild(name + "_Magwell");
            magName = name + "_Magazine";

            ammoCount = 0;
        }

        public void ejectMag()
        {
            if (magazine != null)
            {
                magazine.transform.gameObject.name = "Empty";
                magazine.transform.parent = null;
                magRB.useGravity = true;
                magRB.isKinematic = false;
                magCol.enabled = true;
                magInt.enabled = false;
                Destroy(magazine.gameObject, 10.0f);
                magazine = null;
            }
        }

        private void OnTriggerEnter(Collider magdetect)
        {
            if (magdetect.gameObject.name.Contains(magName) && magazine == null)
            {
                magazine = magdetect.gameObject;

                magInt = magdetect.gameObject.GetComponent<NVRInteractableItem>();
                if (magInt != null)
                {
                    magInt.ForceDetach();
                    magInt.AttachedHand = null;
                }

                magRB = magazine.GetComponent<Rigidbody>();
                if (magRB != null)
                {
                    magRB.useGravity = false;
                    magRB.isKinematic = true;
                }

                magCol = magazine.GetComponent<Collider>();
                if (magCol != null)
                    magCol.enabled = false;

                magazine.transform.parent = magwell;
                magdetect.gameObject.transform.localPosition = new Vector3(0, 0, 0); ;
                magdetect.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 1);
                magdetect.gameObject.transform.localScale = new Vector3(1, 1, 1);

                ammoCount = ammoCapacity;
            }
        }
    }
}
