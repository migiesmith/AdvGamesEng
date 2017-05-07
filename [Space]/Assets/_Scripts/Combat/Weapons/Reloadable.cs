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

        public AmmoReadout readout;

        // Use this for initialization
        void Start()
        {
            ammoCount = 0;
            updateReadout();
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
                        magRB.velocity = Vector3.zero;
                    }
                }
                else
                {
                    Vector2 magProjection = magrail.normalized * magScalar;
                    if (magProjection.magnitude > magrail.magnitude + 0.05f)
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
                        Vector3 localVelocity = transform.InverseTransformDirection(magRB.velocity);
                        Vector2 velocityProjection = magrail.normalized * Vector2.Dot(new Vector2(localVelocity.y, localVelocity.z), magrail.normalized);
                        magRB.velocity = transform.TransformDirection(new Vector3(0.0f, velocityProjection.x, velocityProjection.y));
                    }
                }
            }

            if (animated)
                reloadAnim.SetBool("loaded", ammoCount != 0);
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

            ammoCount = ammoCapacity;
            magIn.Play();

            sliding = false;
            updateReadout();

            if (animated)
                reloadAnim.SetBool("loaded", true);
        }

        public void updateReadout()
        {
            readout.updateAmmoReadout(ammoCount);
        }

        public void updateDecimalReadout()
        {
            if (ammoCount < 0)
                ammoCount = 0;
            readout.updateRoundedReadout(ammoCount);
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

                sliding = true;
            }
        }
    }
}
