using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ConsumableInventory : MonoBehaviour
    {
        public Dictionary<string, int> inventoryList = new Dictionary<string, int>();
        public int startCount = 5;
        // Use this for initialization
        void Start()
        {
            Object[] consumableLoader = Resources.LoadAll("Prefabs/Ammo/", typeof(GameObject));

            foreach (Object consumable in consumableLoader)
            {
                //Debug.Log(consumable);
                inventoryList.Add(consumable.name, startCount);
            }

            consumableLoader = Resources.LoadAll("Prefabs/Consumables/", typeof(GameObject));

            foreach (Object consumable in consumableLoader)
            {
                //Debug.Log(consumable);
                inventoryList.Add(consumable.name, startCount);
            }

            //inventoryList = GameObject.Find("Persistence").GetComponent<Persistence>().transferConsumables();

            Resources.UnloadUnusedAssets();
        }


        public Dictionary<string, int> setConsumables()
        {
            return inventoryList;
        }
    }
}
