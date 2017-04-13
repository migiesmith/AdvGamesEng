using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class FullAuto : MonoBehaviour
    {
        private AR firearm;
        private Reloadable ammoManager;
        private HapticController haptics;

        private float refireDelay;
        private float hapticDuration;

        private float timer;
        private bool hapticLive;
        private bool firing;

        void Start()
        {
            firearm = GetComponent<AR>();
            ammoManager = GetComponent<Reloadable>();
            haptics = GetComponent<HapticController>();
            timer = 0;
            hapticLive = false;
            firing = false;
        }

        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                if (hapticLive)
                {
                    haptics.pulse();
                    if (refireDelay - timer > hapticDuration)
                        hapticLive = false;
                }

                //firearm.disableFX();
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

        public void setParams(float refireDelayIn, float hapticDurationIn)
        {
            refireDelay = refireDelayIn;
            hapticDuration = hapticDurationIn;
        }

        void fire()
        {
            firearm.fire();
            haptics.pulse();
            hapticLive = true;
            timer = refireDelay;
        }

        public void triggerPull()
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
