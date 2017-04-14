using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class BuyAmmo : MonoBehaviour
    {
        private ItemSpawn spawner;
        private DoorSlider slider;

        // Use this for initialization
        void Start()
        {
            spawner = transform.parent.GetComponentInChildren<ItemSpawn>();
            slider = GetComponent<DoorSlider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("z"))
            {
                spawner.buyAmmo();
                slider.open();
            }
            if (slider.getState() == DoorSlider.DoorState.OPEN)
                slider.close();
        }
        void OnTriggerEnter(Collider other)
        {
            if (slider.getState() == DoorSlider.DoorState.CLOSED && other.transform.parent.name.Contains("Hand"))
            {
                spawner.buyAmmo();
                slider.open();
            }
        }
    }
}
