using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ArmoryTile : MonoBehaviour
    {
        private GameObject displayItem;
        private TextMesh count;
        private MeshRenderer rend;
        private ArmoryController controller;
        private DoorSlider slider;

        void Start()
        {
            slider = GetComponent<DoorSlider>();
        }

        void Update()
        {
            if (slider.getState() == DoorSlider.DoorState.OPEN)
                slider.close();
        }

        public void initialise()
        {
            rend = GetComponent<MeshRenderer>();
            controller = transform.parent.GetComponentInChildren<ArmoryController>();
            count = GetComponentInChildren<TextMesh>();
            count.text = "";
            slider = GetComponent<DoorSlider>();
        }

        public void setItem(GameObject toDisplay, int number)
        {
            displayItem = toDisplay;
            rend.material = displayItem.GetComponent<ShopValues>().image;
            count.text = number.ToString();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name.Contains("Hand") && slider.getState() == DoorSlider.DoorState.CLOSED)
            {
                controller.spawnItem(displayItem);
                slider.open();
            }
        }
    }
}
