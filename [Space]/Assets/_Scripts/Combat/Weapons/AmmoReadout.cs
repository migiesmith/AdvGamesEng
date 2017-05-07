using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class AmmoReadout : MonoBehaviour
    {
        public TextMesh readout;

        public void updateAmmoReadout(float ammoCount)
        {
            readout.text = ammoCount.ToString();
        }

        public void updateRoundedReadout(float ammoCount)
        {
            float tempCount = Mathf.RoundToInt(ammoCount * 100.0f)/100.0f;
            readout.text = tempCount.ToString();
        }
    }
}
