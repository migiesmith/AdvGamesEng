using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(TwoHandedInteractableItem)), RequireComponent(typeof(Reloadable))]
    public class Shotgun : MonoBehaviour
    {
        // Weapon object & components
        private TwoHandedInteractableItem gun;
        private Rigidbody gunRB;
        private Transform muzzle;
        private Light glow;
        private ParticleSystem muzzleFlash;
        private ParticleSystem impactSprite;
        private Reloadable ammoManager;
        private DecalParticles decal;
        public AudioSource gunshot;
        private Recoil2H recoil;

        // DPS and firing mode settings
        public float actualDPS = 50.0f;
        public float refireDelay = 0.1f;
        public int pelletsPerShot = 8;
        public float appliedForce = 5.0f;
        public float recoilForce = 15.0f;
        public float optimumRange = 6.0f;
        public float maxRange = 12.0f;
        public float coneRadius = 1.0f;

        // Derived DPS and state durations
        private float weaponDamage;

        // State timers & counters
        private float timer;

        // Hitreg components
        RaycastHit hitInfo;

        // Haptic strength
        public ushort hapticStrength = 2000;
        public float hapticDuration = 0.1f;
        private bool hapticLive;

        // Acquire components, set line renderer parameters, derive damage and timer values from settings, initialise timer and state 
        void Start()
        {
            gun = this.GetComponent<TwoHandedInteractableItem>();
            gunRB = GetComponentInChildren<Rigidbody>();
            muzzle = transform.FindChild(name + "_Muzzle");
            glow = muzzle.GetComponent<Light>();
            muzzleFlash = muzzle.GetComponent<ParticleSystem>();
            impactSprite = transform.FindChild(name + "_Impact").GetComponent<ParticleSystem>();
            ammoManager = GetComponent<Reloadable>();
            decal = GetComponentInChildren<DecalParticles>();
            recoil = GetComponent<Recoil2H>();

            weaponDamage = actualDPS * refireDelay / pelletsPerShot;

            timer = 0.0f;
            hapticLive = false;
        }

        // Decrement valid timers, call pulse control if burst sequence active
        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                if (hapticLive)
                    hapticController();

                if (glow.enabled == true)
                    glow.enabled = false;
            }
        }

        // Toggle on VFX, perform hitreg and apply damage and/or decals
        void fire()
        {
            glow.enabled = true;
            gunshot.Play();
            muzzleFlash.Play();

            for (int i = 0; i < pelletsPerShot; ++i)
            {
                float dist = Mathf.Abs(Random.Range(-0.5f*coneRadius, coneRadius));
                float theta = Random.Range(0.0f, 359.999f);
                Vector3 spread = Quaternion.AngleAxis(theta, muzzle.transform.forward) * (dist * Vector3.Normalize(muzzle.transform.right));

                if (Physics.Raycast(muzzle.transform.position, (muzzle.transform.forward*maxRange+spread), out hitInfo, 1000))
                {
                    impactSprite.transform.position = hitInfo.point;
                    impactSprite.Emit(1);
                    if (hitInfo.transform.gameObject.isStatic)
                        decal.spawnDecal(hitInfo.point, hitInfo.normal);

                    Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                    HealthBar targetHealth = hitInfo.transform.gameObject.GetComponent<HealthBar>();

                    if (targetRB != null)
                        targetRB.AddForce(muzzle.transform.forward * appliedForce);

                    if (targetHealth != null)
                    {
                        float dropOff = 1.0f;

                        if (hitInfo.distance > optimumRange && hitInfo.distance < maxRange)
                            dropOff -= (hitInfo.distance - optimumRange) / (maxRange - optimumRange);
                        else if (hitInfo.distance >= maxRange)
                            dropOff = 0.0f;

                        targetHealth.TakeDamage(weaponDamage * dropOff);
                    }                                   
                }
            }
            gun.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
            if (gun.SecondAttachedHand != null)
                gun.SecondAttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
            --ammoManager.ammoCount;
            timer = refireDelay;

            hapticLive = true;
            hapticController();

            recoil.recoilStart();
        }

        void hapticController()
        {
            gun.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
            if (gun.SecondAttachedHand != null)
                gun.SecondAttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);

            if (refireDelay - timer > hapticDuration)
                hapticLive = false;
        }

        // Detect trigger presses, activate burst sequence if refire delay has elapsed
        public virtual void triggerPull()
        {
            if (ammoManager.ammoCount <= 0)
            {
                ammoManager.ejectMag();
            }
            else if (timer <= 0)
                fire();
        }

        public virtual void dropped()
        {
            glow.enabled = false;
        }
    }
}

