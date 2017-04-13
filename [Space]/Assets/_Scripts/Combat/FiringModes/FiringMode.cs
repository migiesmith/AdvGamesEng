using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class FiringMode : MonoBehaviour
    { 
        protected Firearm firearm;
        protected Reloadable ammoManager;
        protected HapticController haptics;

        protected float refireDelay;
        protected float hapticDuration;

        protected float timer;
        protected bool hapticLive;

/*        void Start()
        {
            firearm = GetComponent<Firearm>();
            ammoManager = GetComponent<Reloadable>();
            timer = 0;
            hapticLive = false;
        }*/

        public void setParams(float refireDelayIn, float hapticDurationIn)
        {
            refireDelay = refireDelayIn;
            hapticDuration = hapticDurationIn;
        }
    }
}
