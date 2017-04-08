using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Firearm : MonoBehaviour
    {
        private Rigidbody gunRB;
        private Transform muzzle;
        private Light glow;
        private ParticleSystem muzzleFlash;
        private ParticleSystem impactSprite;

        public float actualDPS;
        public float refireDelay;
        public float appliedForce;
        public ushort hapticDuration;

        private float weaponDamage;

        private float timer;

        RaycastHit hitInfo;

        // Use this for initialization
        void Start()
        {
            gunRB = GetComponent<Rigidbody>();
            muzzle = transform.FindChild(name + "_Muzzle");
            glow = muzzle.GetComponent<Light>();
            muzzleFlash = muzzle.GetComponent<ParticleSystem>();
            impactSprite = transform.FindChild(name + "_Impact").GetComponent<ParticleSystem>();

            timer = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void fire()
        {
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

        }
    }
}
