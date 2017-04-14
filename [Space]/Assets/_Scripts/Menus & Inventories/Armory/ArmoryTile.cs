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

        public void initialise()
        {
            rend = GetComponent<MeshRenderer>();
            controller = transform.parent.GetComponentInChildren<ArmoryController>();
            count = GetComponentInChildren<TextMesh>();
            count.text = "";
        }

        public void setItem(GameObject toDisplay, int number)
        {
            displayItem = toDisplay;
            rend.material = displayItem.GetComponent<ShopValues>().image;
            count.text = number.ToString();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.name.Contains("Hand"))
                controller.spawnItem(displayItem);
        }
    }
}
