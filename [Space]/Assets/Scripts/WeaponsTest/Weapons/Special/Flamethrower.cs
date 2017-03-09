using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(TwoHandedInteractableItem)), RequireComponent(typeof(Reloadable))]
    public class Flamethrower : MonoBehaviour
    {
        private TwoHandedInteractableItem gun;
        private Rigidbody gunRB;
        private Transform muzzle;
        private ParticleSystem muzzleFlash;
        private Reloadable ammoManager;

        public float actualDPS;
        private bool firing;

        public ushort hapticStrength = 2000;

        // Use this for initialization
        void Start()
        {
            gun = this.GetComponent<TwoHandedInteractableItem>();
            gunRB = GetComponentInChildren<Rigidbody>();
            muzzle = transform.FindChild(name + "_Muzzle");
            muzzleFlash = muzzle.GetComponent<ParticleSystem>();
            ammoManager = GetComponent<Reloadable>();

            firing = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (firing)
            {
                if (gun.AttachedHand != null)
                    gun.AttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
                if (gun.SecondAttachedHand != null)
                    gun.SecondAttachedHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);

                ammoManager.ammoCount -= Time.deltaTime;
                if (ammoManager.ammoCount <= 0)
                {
                    flameOff();
                    firing = false;
                    ammoManager.ejectMag();
                }
            }
        }

        void flameOn()
        {
            muzzleFlash.Play();
        }

        void flameOff()
        {
            muzzleFlash.Stop();
            firing = false;
        }

        public virtual void triggerPull()
        {
            if (ammoManager.ammoCount <= 0)
                ammoManager.ejectMag();
            else
            {
                firing = true;
                flameOn();
            }
        }

        public virtual void triggerRelease()
        {
            if (firing)
                flameOff();
        }

        public virtual void dropped()
        {
            if(firing)
                flameOff();
        }
    }
}
