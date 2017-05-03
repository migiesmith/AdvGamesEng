using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Reloadable : MonoBehaviour
    {
        private Transform magwell;
        private Transform magentry;
        private Vector2 magrail;
        public Collider mainCol;

        public float ammoCapacity = 24;
        public float ammoCount;

        public GameObject magPrefab;
        private GameObject magazine;

        private Rigidbody magRB;
        private NVRInteractableItem magInt;
        private Collider magCol;

        public AudioSource magIn;
        public AudioSource magOut;

        private bool sliding;

        public Animator reloadAnim;
        bool animated;

        // Use this for initialization
        void Start()
        {
            ammoCount = 0;
            magwell = transform.FindDeepChild(name + "_Magwell");
            magentry = magwell.transform.FindChild(name + "_Magentry");
            magrail = new Vector2(magentry.localPosition.y, magentry.localPosition.z);
            sliding = false;

            if (reloadAnim != null)
                animated = true;
            else
                animated = false;
        }

        void Update()
        {
            if (sliding)
            {
                float magScalar = Vector2.Dot(new Vector2(magazine.transform.localPosition.y, magazine.transform.localPosition.z), magrail.normalized);
                if (magScalar <= 0)
                {
                    if (magazine.name != "Empty")
                        reload();
                    else
                    {
                        magazine.transform.localPosition = Vector3.zero;
                        magazine.transform.localRotation = Quaternion.identity;
                    }
                }
                else
                {
                    Vector2 magProjection = magrail.normalized * magScalar;
                    if (magProjection.magnitude > magrail.magnitude + 0.1f)
                    {
                        Physics.IgnoreCollision(mainCol, magCol, false);
                        magazine.transform.parent = null;
                        magazine = null;
                        sliding = false;
                    }
                    else
                    {
                        magazine.transform.localPosition = new Vector3(0.0f, magProjection.x, magProjection.y);
                        magazine.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                    }
                }
                Vector3 magRailDir = new Vector3(0.0f, magrail.x, magrail.y).normalized;
                magRB.velocity = magRailDir * Vector3.Dot(magRailDir, magRB.velocity);
            }
        }

        public void ejectMag()
        {
            if (magazine != null && !sliding)
            {
                magazine.name = "Empty";
                magRB.useGravity = true;
                magRB.isKinematic = false;
                Destroy(magazine.gameObject, 10.0f);
                magOut.Play();
                sliding = true;

                if (animated)
                    reloadAnim.SetBool("loaded", false);
            }
        }

        public void reload()
        {
            if (magInt != null)
            {
                magInt.ForceDetach();
                magInt.AttachedHand = null;
                magInt.enabled = false;
            }
            if (magRB != null)
            {
                magRB.useGravity = false;
                magRB.isKinematic = true;
            }
            magazine.transform.localPosition = new Vector3(0, 0, 0); ;
            magazine.transform.localRotation = new Quaternion(0, 0, 0, 1);
//          magazine.transform.localScale = new Vector3(1, 1, 1);

            ammoCount = ammoCapacity;
            magIn.Play();

            sliding = false;

            if (animated)
                reloadAnim.SetBool("loaded", true);
    }

        private void OnTriggerEnter(Collider magDetect)
        {
            if (magDetect.gameObject.name.Equals(magPrefab.name) && magazine == null)
            {
                magazine = magDetect.gameObject;
                magInt = magazine.GetComponent<NVRInteractableItem>();
                magRB = magazine.GetComponent<Rigidbody>();
                magCol = magazine.GetComponent<Collider>();

                Physics.IgnoreCollision(mainCol, magCol);
                magazine.transform.parent = magwell;
                magazine.transform.localPosition = magentry.transform.localPosition;
                magazine.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
//                magazine.transform.localScale = Vector3.one;

                sliding = true;
            }
        }
    }
}
