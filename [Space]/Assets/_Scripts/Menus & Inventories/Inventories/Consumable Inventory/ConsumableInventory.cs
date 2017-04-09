using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ConsumableInventory : MonoBehaviour
    {
        private NVRHead head;
        public Dictionary<GameObject, int> inventoryList = new Dictionary<GameObject, int>();
        public GameObject[] inventoryItems;
        public int startCount = 5;
        // Use this for initialization
        void Start()
        {
            head = transform.root.GetComponentInChildren<NVRHead>();
            foreach(GameObject g in inventoryItems)
            {
                inventoryList.Add(g, startCount);
            }
        }

        void Update()
        {
            transform.localPosition = new Vector3(head.transform.localPosition.x, transform.localPosition.y, head.transform.localPosition.z);
            transform.localRotation = new Quaternion(transform.localRotation.x, head.transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
        }

        public Dictionary<GameObject, int> setConsumables()
        {
            return inventoryList;
        }
    }
}
