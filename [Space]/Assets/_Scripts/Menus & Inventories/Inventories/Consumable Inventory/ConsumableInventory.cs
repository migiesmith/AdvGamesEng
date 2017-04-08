using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ConsumableInventory : MonoBehaviour
    {
        public Dictionary<GameObject, int> inventoryList = new Dictionary<GameObject, int>();
        public GameObject[] inventoryItems;
        public int startCount = 5;
        // Use this for initialization
        void Start()
        {
            foreach(GameObject g in inventoryItems)
            {
                inventoryList.Add(g, startCount);
            }
        }

        public Dictionary<GameObject, int> setConsumables()
        {
            return inventoryList;
        }
    }
}
