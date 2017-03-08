using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class AmmoReadout : MonoBehaviour
    {
        TextMesh readout;

        void updateAmmoReadout(string ammoCount)
        {
            readout.text = ammoCount;
        }
    }
}
