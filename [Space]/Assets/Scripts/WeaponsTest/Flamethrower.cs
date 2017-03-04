using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractable))]
    public class Flamethrower : MonoBehaviour
    {
        private NVRInteractable gun;
        private Rigidbody gunRB;
        public Transform muzzle;
        public Transform magwell;
        private ParticleSystem muzzleFlash;
        //        public ParticleSystem impactSprite;

        public float actualDPS;
        private bool firing;

        //public Decal scorch;

        public int ammoCapacity;
        private float ammoCount;
        public string magName;
        private GameObject magazine;
        private Rigidbody magRB;
        private NVRInteractableItem magInt;
        private Collider magCol;

        // Use this for initialization
        void Start()
        {
            gun = this.GetComponent<NVRInteractable>();
            gunRB = GetComponentInChildren<Rigidbody>();
            muzzleFlash = muzzle.GetComponent<ParticleSystem>();

            magName = this.transform.root.name + "_Magazine";

            firing = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (firing)
            {
                gun.AttachedHand.TriggerHapticPulse(200, NVRButtons.Touchpad);
                ammoCount -= Time.deltaTime;
                if (ammoCount <= 0)
                {
                    flameOff();
                    firing = false;
                }
            }
        }

        void flameOn()
        {
            muzzleFlash.Play();
        }

        void flameOff()
        {
            muzzleFlash.Stop();
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

        public virtual void triggerPull()
        {
            if (ammoCount <= 0)
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
            else
            {
                firing = true;
                flameOn();
            }
        }

        public virtual void triggerRelease()
        {
            if(firing)
                flameOff();
        }

        public virtual void dropped()
        {
            if(firing)
                flameOff();
        }
    }
}
