using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractable))]
    public class Railgun : MonoBehaviour
    {

        private NVRInteractable gun;
        private Rigidbody gunRB;
        public Transform muzzle;
        public Transform magwell;
        private LineRenderer tracer;
        private Light glow;
        public ParticleSystem impactSprite;
        public ParticleSystem chargeUp;
        public ParticleSystem discharge;

        public float damagePerShot = 100.0f;
        public float appliedForce = 20.0f;
        public float recoilForce = 20.0f;
        public float chargeTime = 2.0f;
        public float refireDelay = 1.0f;

        private float timer;

        RaycastHit hitInfo;

        public int ammoCapacity = 5;
        private int ammoCount;
        public string magName;
        private GameObject magazine;
        private Rigidbody magRB;
        private NVRInteractableItem magInt;
        private Collider magCol;

        private bool isCharging;
        private bool cooldown;

        // Use this for initialization
        void Start()
        {
            gun = this.GetComponent<NVRInteractable>();
            gunRB = GetComponentInChildren<Rigidbody>();
            tracer = muzzle.GetComponent<LineRenderer>();
            glow = muzzle.GetComponent<Light>();

            tracer.numPositions = 2;
            tracer.enabled = false;

            timer = chargeTime;
            isCharging = false;
            cooldown = false;

            magName = this.transform.root.name + "_Magazine";
        }

        // Update is called once per frame
        void Update()
        {
            if (tracer.enabled)
                tracer.enabled = false;

            if (glow.enabled)
                glow.enabled = false;

            if (cooldown)
            {
                if (timer <= 0)
                {
                    cooldown = false;
                    timer = chargeTime;
                }
                else
                    timer -= Time.deltaTime;
            }
            else if (timer > 0)
            {
                if (timer < chargeTime)
                {
                    int emissionRate = (int)(200 * Time.deltaTime * (chargeTime - timer) / chargeTime);
                    chargeUp.Emit(emissionRate);
//                    ushort hapticPWM = (ushort)(500 + 500 * emissionRate);
                    gun.AttachedHand.TriggerHapticPulse(500, NVRButtons.Touchpad);
                }

                if (isCharging)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer > chargeTime)
                    {
                        timer = chargeTime;
                    }
                }
            }
            else if (timer <= 0)
                fire();
        }

        void fire()
        {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hitInfo, 1000))
            {
                tracer.SetPositions(new Vector3[] { muzzle.transform.position, hitInfo.point });
//                tracer.material.mainTextureOffset = new Vector2(-Random.value, 0);
                tracer.enabled = true;
                glow.enabled = true;
                chargeUp.Clear();
                discharge.Play();
                impactSprite.transform.position = hitInfo.point;
                impactSprite.Play();

                Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                HealthBar targetHealth = hitInfo.transform.gameObject.GetComponent<HealthBar>();

                if (targetRB != null)
                    targetRB.AddForce(muzzle.transform.forward * appliedForce);

                if (targetHealth != null)
                    targetHealth.TakeDamage(damagePerShot);

                gun.AttachedHand.TriggerHapticPulse(2999, NVRButtons.Touchpad);
                gunRB.angularVelocity += new Vector3(-recoilForce, 0, 0);
                --ammoCount;
                timer = refireDelay;
                cooldown = true;
                isCharging = false;
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
            else if (!cooldown)
                isCharging = true;
        }

        public virtual void triggerRelease()
        {
            isCharging = false;
        }

        public virtual void dropped()
        {
            isCharging = false;
            timer = chargeTime;
        }
    }
}