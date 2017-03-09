﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(TwoHandedInteractableItem)), RequireComponent(typeof(Reloadable))]
    public class Carbine : MonoBehaviour
    {
        // Weapon object & components
        private TwoHandedInteractableItem gun;
        private Rigidbody gunRB;
        private Transform muzzle;
        private LineRenderer tracer;
        private Light glow;
        private ParticleSystem muzzleFlash;
        private ParticleSystem impactSprite;
        private Reloadable ammoManager;

        // Weapon behaviour settings
        public float actualDPS = 60.0f;
        public float refireDelay = 0.1f;
        public float appliedForce = 5.0f;
        public float recoilForce = 20.0f;
        public float burstDelay = 0.2f;
        public int shotsPerBurst = 5;

        // Derived damage per tick variable
        private float weaponDamage;

        // State timer, counter & boolean
        private float timer;
        private int shotCount;
        private bool firing;

        // Hitreg components
        RaycastHit hitInfo;

        // Haptic strength
        public ushort hapticStrength = 2000;

        // Acquire components, set line renderer parameters, derive damage and timer values, initialise timer and state
        void Start()
        {
            gun = GetComponent<TwoHandedInteractableItem>();
            gunRB = GetComponent<Rigidbody>();
            muzzle = transform.FindChild(name + "_Muzzle");
            tracer = muzzle.GetComponent<LineRenderer>();
            glow = muzzle.GetComponent<Light>();
            muzzleFlash = muzzle.GetComponent<ParticleSystem>();
            impactSprite = transform.FindChild(name + "_Impact").GetComponent<ParticleSystem>();
            ammoManager = GetComponent<Reloadable>();

            tracer.numPositions = 2;
            tracer.enabled = false;

            weaponDamage = actualDPS * (shotsPerBurst * refireDelay + burstDelay)/shotsPerBurst;

            timer = 0.0f;
            shotCount = 0;
            firing = false;
        }

        // Keep time, disable muzzle effects if active
        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (tracer.enabled == true)
                    tracer.enabled = false;
                if (glow.enabled == true)
                    glow.enabled = false;
            }
            else if (firing)
            {
                if (ammoManager.ammoCount > 0)
                    burstController();
                else
                {
                    firing = false;
                    shotCount = 0;
                    ammoManager.ejectMag();
                }
            }
        }

        private void burstController()
        {
            if (shotCount > 0)
                fire();
            else
            {
                firing = false;
                timer = burstDelay;
            }
        }

        void fire()
        {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hitInfo, 1000))
            {
                tracer.SetPositions(new Vector3[] { muzzle.transform.position, hitInfo.point });
                tracer.material.mainTextureOffset = new Vector2(-Random.value, 0);
                tracer.enabled = true;
                glow.enabled = true;
                muzzleFlash.Play();
                impactSprite.transform.position = hitInfo.point;
                impactSprite.Play();

                Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                HealthBar targetHealth = hitInfo.transform.gameObject.GetComponent<HealthBar>();

                if (targetRB != null)
                    targetRB.AddForce(muzzle.transform.forward * appliedForce);

                if (targetHealth != null)
                    targetHealth.TakeDamage(weaponDamage);

                gun.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
                if (gun.SecondAttachedHand != null)
                    gun.SecondAttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);

                gunRB.angularVelocity += new Vector3(-recoilForce, 0, 0);
                --ammoManager.ammoCount;
                --shotCount;
                timer = refireDelay;
            }
        }

        public virtual void triggerPull()
        {
            if (ammoManager.ammoCount <= 0)
                ammoManager.ejectMag();
            else if (!firing && timer <= 0)
            {
                firing = true;
                shotCount = shotsPerBurst;
                fire();
            }
        }

        public virtual void dropped ()
        {
            if (firing)
            {
                firing = false;
                shotCount = 0;
            }
        }
    }

}

