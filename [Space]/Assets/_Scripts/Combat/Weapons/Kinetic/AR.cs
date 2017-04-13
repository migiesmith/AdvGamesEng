using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(TwoHandedInteractableItem), typeof(Reloadable), typeof(FullAuto))]
    public class AR : MonoBehaviour
    {
        private FullAuto fireControl;
        private DamageController damageControl;
        // Weapon object & components
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

        // Hitreg components
        RaycastHit hitInfo;

        // Haptic strength
        public ushort hapticStrength = 2000;
        public float hapticDuration = 0.1f;

        // Acquire components, set line renderer parameters, derive damage and timer values, initialise timer and state
        void Start()
        {
            weaponDamage = actualDPS * refireDelay;

            fireControl = GetComponent<FullAuto>();
            fireControl.setParams(refireDelay, hapticDuration);

            GetComponent<HapticController>().setParams(hapticStrength);

            damageControl = GetComponent<DamageController>();
            damageControl.setParams(weaponDamage);

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
        }

        public void fire()
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

                damageControl.hitTarget(hitInfo.transform.gameObject, hitInfo.point);

                --ammoManager.ammoCount;

                recoil.recoilStart();
            }
        }
    }
}
