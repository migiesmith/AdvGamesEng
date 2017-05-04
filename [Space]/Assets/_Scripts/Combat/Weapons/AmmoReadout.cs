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
    }
}
