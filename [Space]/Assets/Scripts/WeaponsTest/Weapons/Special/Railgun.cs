using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(TwoHandedInteractableItem)), RequireComponent(typeof(Reloadable))]
    public class Railgun : MonoBehaviour
    {
        private TwoHandedInteractableItem gun;
        private Rigidbody gunRB;
        private Transform muzzle;
        private LineRenderer tracer;
        private Light glow;
        private ParticleSystem impactSprite;
        private ParticleSystem chargeUp;
        private ParticleSystem discharge;
        private Reloadable ammoManager;
        private DecalParticles decal;
        public AudioSource release;
        public AudioSource build;
        private Recoil2H recoil;

        public float damagePerShot = 100.0f;
        public float appliedForce = 20.0f;
        public float recoilForce = 20.0f;
        public float chargeTime = 2.0f;
        public float refireDelay = 1.0f;

        private float timer;

        RaycastHit hitInfo;

        public ushort chargeHapticStrength = 2000;
        public ushort dischargeHapticStrength = 2999;

        public int maxEmissionRate = 200;

        private bool isCharging;
        private bool cooldown;

        // Use this for initialization
        void Start()
        {
            gun = this.GetComponent<TwoHandedInteractableItem>();
            gunRB = GetComponentInChildren<Rigidbody>();
            muzzle = transform.FindChild(name + "_Muzzle");
            tracer = muzzle.GetComponent<LineRenderer>();
            glow = muzzle.GetComponent<Light>();
            impactSprite = transform.FindChild(name + "_Impact").GetComponent<ParticleSystem>();
            chargeUp = transform.FindChild(name + "_Charge").GetComponent<ParticleSystem>();
            discharge = transform.FindChild(name + "_Discharge").GetComponent<ParticleSystem>();
            ammoManager = GetComponent<Reloadable>();
            decal = GetComponentInChildren<DecalParticles>();
            recoil = GetComponent<Recoil2H>();

            tracer.numPositions = 2;
            tracer.enabled = false;

            timer = chargeTime;
            isCharging = false;
            cooldown = false;
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
                    float chargeFactor = (chargeTime - timer) / chargeTime;
                    int emissionRate = (int)(maxEmissionRate * Time.deltaTime * chargeFactor);
                    chargeUp.Emit(emissionRate);
                    ushort hapticPWM = (ushort)(chargeHapticStrength * chargeFactor);
                    gun.AttachedHand.TriggerHapticPulse(hapticPWM, NVRButtons.Touchpad);
                    if (gun.SecondAttachedHand != null)
                        gun.SecondAttachedHand.TriggerHapticPulse(hapticPWM, NVRButtons.Touchpad);
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
                tracer.enabled = true;
                glow.enabled = true;
                release.Play();
                chargeUp.Clear();
                discharge.Play();
                impactSprite.transform.position = hitInfo.point;
                impactSprite.Play();
                if (hitInfo.transform.gameObject.isStatic)
                    decal.spawnDecal(hitInfo.point, hitInfo.normal);

                Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                HealthBar targetHealth = hitInfo.transform.gameObject.GetComponent<HealthBar>();

                if (targetRB != null)
                    targetRB.AddForce(muzzle.transform.forward * appliedForce);

                if (targetHealth != null)
                    targetHealth.TakeDamage(damagePerShot);

                gun.AttachedHand.TriggerHapticPulse(dischargeHapticStrength, NVRButtons.Touchpad);
                --ammoManager.ammoCount;
                timer = refireDelay;
                cooldown = true;
                isCharging = false;

                recoil.recoilStart();
            }
        }

        public virtual void triggerPull()
        {
            if (ammoManager.ammoCount <= 0)
                ammoManager.ejectMag();
            else if (!cooldown)
            {
                isCharging = true;
                build.Play();
            }
        }

        public virtual void triggerRelease()
        {
            isCharging = false;
            build.Stop();
        }

        public virtual void dropped()
        {
            isCharging = false;
            build.Stop();
            timer = chargeTime;
        }
    }
}