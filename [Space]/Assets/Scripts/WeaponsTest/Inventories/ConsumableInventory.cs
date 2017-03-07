using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ConsumableInventory : MonoBehaviour
    {
        public Dictionary<string, int> inventoryList = new Dictionary<string, int>();
        // Use this for initialization
        void Start()
        {
            Object[] consumableLoader = Resources.LoadAll("Prefabs/Ammo/", typeof(GameObject));

            foreach (Object consumable in consumableLoader)
            {
                Debug.Log(consumable);
                inventoryList.Add(consumable.name, 5);
            }

            consumableLoader = Resources.LoadAll("Prefabs/Consumables/", typeof(GameObject));

            foreach (Object consumable in consumableLoader)
            {
                Debug.Log(consumable);
                inventoryList.Add(consumable.name, 5);
            }

            Resources.UnloadUnusedAssets();
        }
    }
}
