﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(TwoHandedInteractableItem)), RequireComponent(typeof(Reloadable))]
    public class AssaultRifle : MonoBehaviour
    {
        // Weapon object & components
        private TwoHandedInteractableItem gun;
        private Transform muzzle;
        private LineRenderer tracer;
        private Light glow;
        private ParticleSystem muzzleFlash;
        private ParticleSystem impactSprite;
        private Reloadable ammoManager;
        private DecalParticles decal;
        public AudioSource gunshot;
        private Recoil2H recoil;

        // Weapon behaviour settings
        public float actualDPS = 50.0f;
        public float refireDelay = 0.05f;
        public float appliedForce = 5.0f;
        public float recoilForce = 15.0f;

        // Derived damage per tick variable
        private float weaponDamage;

        // State timer & boolean
        private float timer;
        private bool firing;

        // Hitreg components
        RaycastHit hitInfo;

        // Haptic strength
        public ushort hapticStrength = 2000;
        public float hapticDuration = 0.1f;
        private bool hapticLive;

        // Acquire components, set line renderer parameters, derive damage and timer values, initialise timer and state
        void Start()
        {
            gun = GetComponent<TwoHandedInteractableItem>();
            muzzle = transform.FindChild(name + "_Muzzle");
            tracer = muzzle.GetComponent<LineRenderer>();
            glow = muzzle.GetComponent<Light>();
            muzzleFlash = muzzle.GetComponent<ParticleSystem>();
            impactSprite = transform.FindChild(name + "_Impact").GetComponent<ParticleSystem>();
            ammoManager = GetComponent<Reloadable>();
            decal = GetComponentInChildren<DecalParticles>();
            recoil = GetComponent<Recoil2H>();

            tracer.numPositions = 2;
            tracer.enabled = false;

            weaponDamage = actualDPS * refireDelay;

            timer = 0.0f;
            firing = false;
            hapticLive = false;
        }

        // Keep time, disable muzzle effects if active
        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                if (hapticLive)
                    hapticController();

                if (tracer.enabled == true)
                    tracer.enabled = false;
                if (glow.enabled == true)
                    glow.enabled = false;
            }
            else if (firing)
            {
                if (ammoManager.ammoCount > 0)
                    fire();
                else
                {
                    firing = false;
                    ammoManager.ejectMag();
                }
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
                gunshot.Play();
                muzzleFlash.Play();
                impactSprite.transform.position = hitInfo.point;
                impactSprite.Play();
                if (hitInfo.transform.gameObject.isStatic)
                    decal.spawnDecal(hitInfo.point, hitInfo.normal);

                Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                HealthBar targetHealth = hitInfo.transform.gameObject.GetComponent<HealthBar>();

                if (targetRB != null)
                    targetRB.AddForce(muzzle.transform.forward * appliedForce);

                if (targetHealth != null)
                    targetHealth.TakeDamage(weaponDamage);

                gun.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
                if (gun.SecondAttachedHand != null)
                    gun.SecondAttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);

                --ammoManager.ammoCount;
                timer = refireDelay;

                recoil.recoilStart();

                hapticLive = true;
                hapticController();
            }
        }

        void hapticController()
        {
            gun.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
            if (gun.SecondAttachedHand != null)
                gun.SecondAttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);

            if (refireDelay - timer > hapticDuration)
                hapticLive = false;
        }

        public virtual void triggerPull()
        {
            if (ammoManager.ammoCount <= 0)
                ammoManager.ejectMag();
            else
            {
                firing = true;
                fire();
            }
        }

        public virtual void triggerRelease()
        {
            firing = false;
        }

        public virtual void dropped()
        {
            if (firing)
                firing = false;
        }
    }

}
