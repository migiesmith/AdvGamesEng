using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractable))]
    public class Shotgun : MonoBehaviour
    {
        // Weapon object & components
        private NVRInteractable gun;
        private Rigidbody gunRB;
        public Transform muzzle;
        public Transform magwell;
        private Light glow;
        private ParticleSystem muzzleFlash;
        public ParticleSystem impactSprite;

        // DPS and firing mode settings
        public float actualDPS = 50.0f;
        public float refireDelay = 0.1f;
        public int pelletsPerShot = 8;
        public float appliedForce = 5.0f;
        public float recoilForce = 15.0f;

        // Derived DPS and state durations
        private float weaponDamage;

        // State timers & counters
        private float timer;

        // Hitreg components
        RaycastHit hitInfo;

        // Decals
        public Decal bulletHole;

        // Ammo & reload
        public int ammoCapacity = 24;
        private int ammoCount;
        public string magName;
        private GameObject magazine;
        private Rigidbody magRB;
        private NVRInteractableItem magInt;
        private Collider magCol;

        // Acquire components, set line renderer parameters, derive damage and timer values from settings, initialise timer and state 
        void Start()
        {
            gun = this.GetComponent<NVRInteractable>();
            gunRB = GetComponentInChildren<Rigidbody>();
            glow = muzzle.GetComponent<Light>();
            muzzleFlash = muzzle.GetComponent<ParticleSystem>();

            weaponDamage = actualDPS * refireDelay;

            timer = 0.0f;

            magName = this.transform.root.name + "_Magazine";
        }

        // Decrement valid timers, call pulse control if burst sequence active
        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (glow.enabled == true)
                    glow.enabled = false;
            }
        }

        // Toggle on VFX, perform hitreg and apply damage and/or decals
        void fireBullet()
        {
            glow.enabled = true;
            muzzleFlash.Play();

            for (int i = 0; i < pelletsPerShot; ++i)
            {
                if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hitInfo, 1000))
                {
                    impactSprite.transform.position = hitInfo.point;
                    impactSprite.Play();

                    Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                    HealthBar targetHealth = hitInfo.transform.gameObject.GetComponent<HealthBar>();

                    if (targetRB != null)
                        targetRB.AddForce(muzzle.transform.forward * appliedForce);

                    if (targetHealth != null)
                        targetHealth.TakeDamage(weaponDamage);
                    else
                    {
                        Decal hole = Instantiate(bulletHole, hitInfo.point, Quaternion.FromToRotation(Vector3.back, hitInfo.normal));
                        hole.GetComponent<DecalController>().beginControl = true;
                    }                                         
                }
            }
            gun.AttachedHand.TriggerHapticPulse(2000, NVRButtons.Touchpad);
            gunRB.angularVelocity += new Vector3(-recoilForce, 0, 0);
            --ammoCount;
            timer = refireDelay;
        }

        // Reloading script
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

        // Detect trigger presses, activate burst sequence if refire delay has elapsed
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
            else if (timer <= 0)
                fireBullet();
        }

        public virtual void dropped()
        {
            glow.enabled = false;
        }
    }
}

