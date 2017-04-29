using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ConsumableInventory : MonoBehaviour
    {
        private NVRHead head;
        private ConsumableSlot[] cSlots;
        public Dictionary<GameObject, int> inventoryList = new Dictionary<GameObject, int>();
        public GameObject[] inventoryItems;
        public int startCount = 5;
        // Use this for initialization
        void Start()
        {
            head = transform.root.GetComponentInChildren<NVRHead>();
            cSlots = GetComponentsInChildren<ConsumableSlot>();
            foreach(GameObject g in inventoryItems)
            {
                inventoryList.Add(g, startCount);
            }
        }

        void Update()
        {
            transform.localPosition = new Vector3(head.transform.localPosition.x, transform.localPosition.y, head.transform.localPosition.z);
            transform.localRotation = Quaternion.LookRotation(new Vector3(head.transform.forward.x, 0.0f, head.transform.forward.z), Vector3.up);
        }

        public List<int> setConsumables()
        {
            List<int> itemCount = new List<int>();
            foreach(GameObject g in inventoryItems)
            {
                itemCount.Add(inventoryList[g]);
            }
            return itemCount;
        }

        public void getConsumables(List<int> itemCount)
        {
            int index = 0;
            foreach (GameObject g in inventoryItems)
            {
                inventoryList[g] = itemCount[index];
                ++index;
            }
            foreach (ConsumableSlot cSlot in cSlots)
            {
                if (cSlot.slotItem != null)
                    cSlot.readout.text = inventoryList[cSlot.slotItem].ToString();
            }
        }

        public void updateCount()
        {
            foreach (ConsumableSlot cSlot in cSlots)
            {
                if (cSlot.slotItem != null)
                    cSlot.readout.text = inventoryList[cSlot.slotItem].ToString();
            }
        }
    }
}
