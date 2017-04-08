using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Reloadable : MonoBehaviour
    {
        private Transform magwell;

        public float ammoCapacity = 24;
        public float ammoCount;

        public GameObject magPrefab;
        private GameObject magazine;

        private Rigidbody magRB;
        private NVRInteractableItem magInt;
        private Collider magCol;

        public AudioSource magIn;
        public AudioSource magOut;

        // Use this for initialization
        void Start()
        {
            ammoCount = 0;
            magwell = transform.FindChild(name + "_Magwell");
        }

        public void ejectMag()
        {
            if (magazine != null)
            {
                magazine.name = "Empty";
                magazine.transform.parent = null;
                magRB.useGravity = true;
                magRB.isKinematic = false;
                magCol.enabled = true;
                magInt.enabled = false;
                Destroy(magazine.gameObject, 10.0f);
                magazine = null;
                magOut.Play();
            }
        }

        private void OnTriggerEnter(Collider magDetect)
        {
            if (magDetect.gameObject.name == magPrefab.gameObject.name && magazine == null)
            {
                magazine = magDetect.gameObject;

                magInt = magDetect.gameObject.GetComponent<NVRInteractableItem>();
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
                magDetect.gameObject.transform.localPosition = new Vector3(0, 0, 0); ;
                magDetect.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 1);
                magDetect.gameObject.transform.localScale = new Vector3(1, 1, 1);

                ammoCount = ammoCapacity;
                magIn.Play();
            }
        }
    }
}
