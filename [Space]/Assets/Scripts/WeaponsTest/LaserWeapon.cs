using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractableItem)), RequireComponent(typeof(LineRenderer))]
    public class LaserWeapon : MonoBehaviour
    {
        // Weapon object & components
        NVRInteractableItem gun;
        public Transform muzzle;
        public Transform magwell;
        private LineRenderer beam;
        private Light glow;

        // DPS and firing mode settings
        public float actualDPS = 50.0f;
        public float refireDelay = 0.2f;
        public float burstDuration = 0.3f;
        public int pulsesPerBurst = 5;
        public float pulseDutyRatio = 0.6f;
        public float appliedForce = 5.0f;

        // Derived DPS and state durations
        private float weaponDamage;
        private float pulseOnDuration;
        private float pulseOffDuration;

        // State timers & counters
        private float timer;
        private int pulseCount;

        // State control booleans
        private bool burstActive;
        private bool pulseActive;

        // Hitreg components
        RaycastHit hitInfo;

        // Decals
        public Decal laserBurn;

        // Ammo & reload
        public int ammoCapacity = 24;
        private int ammoCount;
        private GameObject magazine;
        private Rigidbody magRB;
        private NVRInteractableItem magInt;
        private Collider magCol;

        // Acquire components, set line renderer parameters, derive damage and timer values from settings, initialise timer and state 
        void Start()
        {
            gun = GetComponent<NVRInteractableItem>();
            beam = GetComponent<LineRenderer>();
            glow = GetComponentInChildren<Light>();

            NVRHelpers.LineRendererSetWidth(beam, 0.04f, 0.04f);
            beam.numPositions = 2;
            beam.numCapVertices = 4;

            weaponDamage = actualDPS * (refireDelay + burstDuration) / (refireDelay * pulseDutyRatio);
            pulseOnDuration = burstDuration * pulseDutyRatio / pulsesPerBurst ;
            pulseOffDuration = burstDuration * (1 - pulseDutyRatio) / pulsesPerBurst ;

            timer = 0.0f;
            burstActive = false;
            pulseActive = false;
            beamOff();
        }

        // Decrement valid timers, call pulse control if burst sequence active
        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;

            if (burstActive)
                pulseController();
        }

        // Toggle firing effects and damage on and off according to settings and timer
        void pulseController()
        {
            if (pulseActive)
            {
                if (timer <= 0)
                {
                    beamOff();
                    pulseActive = false;
                    timer = pulseOffDuration;
                    --pulseCount;
                }
                else
                    beamOn();
            }
            else
            {
                if (timer <= 0)
                {
                    if (pulseCount <= 0)
                    {
                        burstActive = false;
                        timer = refireDelay;
                    }
                    else
                    {
                        pulseActive = true;
                        timer = pulseOnDuration;
                    }
                }
            }
        }

        // Toggle on VFX, perform hitreg and apply damage and/or decals
        void beamOn()
        {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hitInfo, 1000))
            {
                beam.SetPositions(new Vector3[] { muzzle.transform.position, hitInfo.point });
                beam.material.mainTextureOffset = new Vector2(Time.time, 0);
                beam.enabled = true;
                glow.enabled = true;

                Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                HealthBar targetHealth = hitInfo.transform.gameObject.GetComponent<HealthBar>();

                if (targetRB != null)
                    targetRB.AddForce(muzzle.transform.forward * appliedForce);

                if (targetHealth != null)
                    targetHealth.TakeDamage(weaponDamage * Time.deltaTime);
                else
                {
                    Decal burn = Instantiate(laserBurn, hitInfo.point, Quaternion.FromToRotation(Vector3.back, hitInfo.normal));
                    burn.GetComponent<DecalController>().beginControl = true;
                }
                gun.AttachedHand.TriggerHapticPulse(2000, NVRButtons.Touchpad);
            }
        }

        // Toggle off VFX
        void beamOff()
        {
            beam.enabled = false;
            glow.enabled = false;
        }

        // Reloading script
        private void OnTriggerEnter(Collider magdetect)
        {
            if (magdetect.gameObject.name.Contains("Magazine") && magazine == null)
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
            if (!burstActive && timer <= 0.0f)
            {
                if (ammoCount > 0)
                {
                    timer = pulseOnDuration;
                    pulseCount = pulsesPerBurst;
                    burstActive = true;
                    pulseActive = true;
                    beamOn();
                    --ammoCount;
                }
                else
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
            }
        }

        public virtual void dropped()
        {
            if (burstActive)
            {
                beamOff();
                burstActive = false;
                pulseActive = false;
                timer = 0.0f;
                pulseCount = 0;
            }
        }
    }
}
