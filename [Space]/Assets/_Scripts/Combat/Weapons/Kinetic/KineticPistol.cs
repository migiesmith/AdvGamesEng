﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractableItem)), RequireComponent(typeof(Reloadable))]
    public class KineticPistol : MonoBehaviour
    {
        // Weapon object & components
        private NVRInteractableItem gun;
        private Transform muzzle;
        private LineRenderer tracer;
        private Light glow;
        private ParticleSystem muzzleFlash;
        private ParticleSystem impactSprite;
        private Reloadable ammoManager;
        private DecalParticles decal;

        // Weapon behaviour settings
        public float actualDPS = 60.0f;
        public float refireDelay = 0.1f;
        public float appliedForce = 5.0f;
        public float recoilForce = 20.0f;

        // Derived damage per tick variable
        private float weaponDamage;

        // State timer & boolean
        private float timer;

        // Hitreg components
        RaycastHit hitInfo;

        // Haptic strength
        public ushort hapticStrength = 2000;
        public float hapticDuration = 0.1f;
        private bool hapticLive;

        [System.Serializable] public class HitEvent : UnityEvent<Vector3> { }
        public HitEvent onHit;


        // Acquire components, set line renderer parameters, derive damage and timer values, initialise timer and state
        void Start()
        {
            gun = GetComponent<NVRInteractableItem>();
            muzzle = transform.FindChild(name + "_Muzzle");
            tracer = muzzle.GetComponent<LineRenderer>();
            glow = muzzle.GetComponent<Light>();
            muzzleFlash = muzzle.GetComponent<ParticleSystem>();
            impactSprite = transform.FindChild(name + "_Impact").GetComponent<ParticleSystem>();
            ammoManager = GetComponent<Reloadable>();
            decal = GetComponentInChildren<DecalParticles>();

            tracer.numPositions = 2;
            tracer.enabled = false;

            weaponDamage = actualDPS * refireDelay;

            timer = 0.0f;
            hapticLive = false;
            glow.enabled = false;
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
                if (hitInfo.transform.gameObject.isStatic)
                    decal.spawnDecal(hitInfo.point, hitInfo.normal);

                Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                HealthBar targetHealth = hitInfo.transform.gameObject.GetComponent<HealthBar>();
                ShieldBar targetShield = hitInfo.transform.gameObject.GetComponent<ShieldBar>();

                if (targetRB != null)
                    targetRB.AddForce(muzzle.transform.forward * appliedForce);

                if (targetShield != null)
                {
                    if (!targetShield.down)
                        targetShield.TakeDamage(weaponDamage, hitInfo.point);
                    else if (targetHealth != null)
                        targetHealth.TakeDamage(weaponDamage);
                }
                else if (targetHealth != null)
                    targetHealth.TakeDamage(weaponDamage);

                gun.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);

                --ammoManager.ammoCount;
                timer = refireDelay;

                hapticLive = true;
                hapticController();
                onHit.Invoke(hitInfo.point);
            }
        }

        void hapticController()
        {
            gun.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
            if (refireDelay - timer > hapticDuration)
                hapticLive = false;
        }

        public virtual void triggerPull()
        {
            if (ammoManager.ammoCount <= 0)
                ammoManager.ejectMag();
            else if (timer <= 0)
                fire();
        }
    }

}

