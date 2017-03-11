using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(TwoHandedInteractableItem)), RequireComponent(typeof(Reloadable))]
    public class LaserRifle : MonoBehaviour
    {
        // Weapon object & components
        TwoHandedInteractableItem gun;
        private Transform muzzle;
        private LineRenderer beam;
        private Light glow;
        private Reloadable ammoManager;
        private DecalParticles decal;

        // DPS and firing mode settings
        public float actualDPS = 50.0f;
        public float refireDelay = 0.2f;
        public float burstDuration = 0.3f;
        public int numPulses = 5;
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
        private bool pulseActive;
        private bool beamActive;

        // Hitreg components
        RaycastHit hitInfo;

        // Haptic strength
        public ushort hapticStrength = 2000;

        // Use this for initialization
        void Start()
        {
            gun = GetComponent<TwoHandedInteractableItem>();
            muzzle = transform.FindChild(name + "_Muzzle");
            beam = muzzle.GetComponent<LineRenderer>();
            glow = muzzle.GetComponent<Light>();
            ammoManager = GetComponent<Reloadable>();
            decal = GetComponentInChildren<DecalParticles>();

            beam.numPositions = 2;
            beam.numCapVertices = 4;

            weaponDamage = actualDPS * (refireDelay + burstDuration) / (refireDelay * pulseDutyRatio);
            pulseOnDuration = burstDuration * pulseDutyRatio / numPulses;
            pulseOffDuration = burstDuration * (1 - pulseDutyRatio) / numPulses;

            timer = 0.0f;
            pulseActive = false;
            beamOff();
        }

        // Decrement valid timers, call pulse control if pulse sequence active
        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;

            if (pulseActive)
                pulseController();
        }

        // Toggle firing effects and damage on and off according to settings and timer
        void pulseController()
        {
            if (beamActive)
            {
                if (timer <= 0)
                {
                    beamOff();
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
                        pulseActive = false;
                        timer = refireDelay;
                    }
                    else
                    {
                        beamActive = true;
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
                if (hitInfo.transform.gameObject.isStatic)
                    decal.spawnDecal(hitInfo.point, hitInfo.normal);

                Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                HealthBar targetHealth = hitInfo.transform.gameObject.GetComponent<HealthBar>();

                if (targetRB != null)
                    targetRB.AddForce(muzzle.transform.forward * appliedForce);

                if (targetHealth != null)
                    targetHealth.TakeDamage(weaponDamage * Time.deltaTime);

                gun.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
                if (gun.SecondAttachedHand != null)
                    gun.SecondAttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
            }
        }

        // Toggle off VFX
        void beamOff()
        {
            beamActive = false;
            beam.enabled = false;
            glow.enabled = false;
        }

        // Detect trigger presses, activate burst sequence if refire delay has elapsed
        public virtual void triggerPull()
        {
            if (ammoManager.ammoCount <= 0)
                ammoManager.ejectMag();
            else if (!pulseActive && timer <= 0.0f)
            {
                timer = pulseOnDuration;
                pulseCount = numPulses;
                pulseActive = true;
                beamActive = true;
                beamOn();
                --ammoManager.ammoCount;
            }
        }

        public virtual void dropped()
        {
            if (pulseActive)
            {
                beamOff();
                pulseActive = false;
                timer = 0.0f;
                pulseCount = 0;
            }
        }
    }
}

