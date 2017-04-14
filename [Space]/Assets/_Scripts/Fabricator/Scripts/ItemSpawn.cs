using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ItemSpawn : MonoBehaviour
    {
        // Text meshes to display item details
        public TextMesh itemDescription;
        public TextMesh weaponStats;
        public TextMesh ammoCount;

        // Position of spawned items and dissolve script parameters
        public Transform spawnLocation;
        public Gradient dissolveGradient;
        public Texture dissolveTex;

        // Item database and player consumable inventory
        private PrefabDatabase prefabDB;
        private ConsumableInventory consumableInventory;

        // Reference to magazine prefab for currently-selected weapons
        private GameObject currAmmo;

        // Reference to ammo button object for show/hide functionality
        private GameObject ammoButton;

        // Find components, initialise textmesh content & deactivate ammo button
        void Start()
        {
            prefabDB = GetComponent<PrefabDatabase>();
            consumableInventory = FindObjectOfType<ConsumableInventory>();

            itemDescription.text = "";
    //      weaponStats.text = "";
            ammoCount.text = "";

            ammoButton = transform.parent.GetComponentInChildren<BuyAmmo>().gameObject;
            ammoButton.SetActive(false);
        }

        // If passed prefab is a consumable, adds it directly to consumable inventory,
        // otherwise, spawns the passed prefab with animation provided by Dissolve
        public void spawn(GameObject toSpawn)
        {
            if (consumableInventory.inventoryList.ContainsKey(toSpawn))
            {
                ++consumableInventory.inventoryList[toSpawn];
                consumableInventory.updateCount();
            }
            else
            {
                GameObject item = Instantiate(toSpawn, spawnLocation.position, spawnLocation.rotation);
                item.name = toSpawn.name;

                Rigidbody itemRB = item.GetComponentInChildren<Rigidbody>();
                if (itemRB != null)
                {
                    itemRB.useGravity = false;
                    itemRB.isKinematic = true;
                }
                item.GetComponent<NVRInteractable>().enabled = false;

                DissolveController dissolve = item.AddComponent<DissolveController>();
                dissolve.setAndDissolve(dissolveGradient, dissolveTex);
            }
        }

        public void buyAmmo()
        {
            if (consumableInventory.inventoryList.ContainsKey(currAmmo))
            {
                currAmmo.GetComponent<ShopValues>().buy();
                ++consumableInventory.inventoryList[currAmmo];
                ammoCount.text = consumableInventory.inventoryList[currAmmo].ToString();
            }
        }

        public void updateDetails(GameObject currItem)
        {
            itemDescription.text = currItem.GetComponent<ShopValues>().description;
            if (currItem.GetComponent<Reloadable>() != null)
            {
                currAmmo = prefabDB.getPrefab(currItem.name + "_Magazine");
                ammoButton.SetActive(true);
                ammoCount.text = consumableInventory.inventoryList[currAmmo].ToString();
            }
            else
            {
                currAmmo = null;
                ammoButton.SetActive(false);
                ammoCount.text = "";
            }
        }
    }
}
