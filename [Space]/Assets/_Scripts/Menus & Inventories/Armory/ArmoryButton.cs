using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ArmoryButton : MonoBehaviour
    {
        private DoorSlider slider;

        // Use this for initialization
        void Start()
        {
            slider = GetComponent<DoorSlider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (slider.getState() == DoorSlider.DoorState.OPEN)
                slider.close();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name.Contains("Hand") && slider.getState() == DoorSlider.DoorState.CLOSED)
                slider.open();
        }
    }
}