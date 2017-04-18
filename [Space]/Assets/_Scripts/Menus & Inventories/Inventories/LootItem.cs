using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractable), typeof(ShopValues))]
    public class LootItem : MonoBehaviour
    {
        public void addToInventory()
        {
            if (FindObjectOfType<LootInventory>().addLoot(transform.root.gameObject))
                Destroy(transform.root.gameObject);
        }
    }
}
