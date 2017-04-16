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
        public TextMesh ammoCount;
        public ResourceCost purchaseCost;
        public ResourceCost ammoCost;
        public ResourceDisplay playerResources;
        public ResourceDisplay saleValue;

        // Position of spawned items and dissolve script parameters
        public Transform spawnLocation;
        public Gradient dissolveGradient;
        public Texture dissolveTex;

        // Item database and player consumable inventory
        public PrefabDatabase prefabDB;
        private ConsumableInventory consumableInventory;
        private LootInventory lootInventory;
        private Currency playerVals;

        // Reference to magazine prefab for currently-selected weapons
        private GameObject currAmmo;

        // Reference to ammo button object for show/hide functionality
        public GameObject ammoButton;

        private ShopValues currVals;
        private ShopValues ammoVals;


        // Find components, initialise textmesh content & deactivate ammo button
        void Start()
        {
            consumableInventory = FindObjectOfType<ConsumableInventory>();
            lootInventory = FindObjectOfType<LootInventory>();
            playerVals = FindObjectOfType<Currency>();

            itemDescription.text = "";
            ammoCount.text = "";

            ammoButton = transform.parent.GetComponentInChildren<BuyAmmo>().gameObject;
            ammoButton.SetActive(false);

            updateResources();
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
                dissolve.setAndDissolveIn(dissolveGradient, dissolveTex);
            }
            updateResources();
        }

        public void buyAmmo()
        {
            List<int> available = playerVals.getCurrency();
            List<int> required = ammoVals.getVals();
            if (available[0] >= required[0] && available[1] >= required[1] && available[2] >= required[2] && available[3] >= required[3] && consumableInventory.inventoryList.ContainsKey(currAmmo))
            {
                currAmmo.GetComponent<ShopValues>().buy();
                ++consumableInventory.inventoryList[currAmmo];
                ammoCount.text = consumableInventory.inventoryList[currAmmo].ToString();
                updateResources();
            }
        }

        public void updateDetails(GameObject currItem)
        {
            currVals = currItem.GetComponent<ShopValues>();
            itemDescription.text = currVals.description.Replace("\\n","\n");
            purchaseCost.updateCost(currVals.getVals());
            if (currItem.GetComponent<Reloadable>() != null)
            {
                currAmmo = prefabDB.getPrefab(currItem.name + "_Magazine");
                ammoVals = currAmmo.GetComponent<ShopValues>();
                ammoButton.SetActive(true);
                ammoCost.updateCost(ammoVals.getVals());
                if (consumableInventory != null)
                    ammoCount.text = consumableInventory.inventoryList[currAmmo].ToString();
            }
            else
            {
                currAmmo = null;
                ammoButton.SetActive(false);
                ammoCount.text = "";
            }
            updateResources();
        }

        public void updateResources()
        {
            if (lootInventory != null)
                saleValue.updateCost(lootInventory.totalValue());
            if (playerVals != null)
            playerResources.updateCost(playerVals.getCurrency());
            if (currVals != null)
                purchaseCost.updateCost(currVals.getVals());
            if (ammoCost.isActiveAndEnabled)
                ammoCost.updateCost(ammoVals.getVals());
        }

        public void dissolveOut(GameObject toDissolve)
        {
            DissolveController dissolve = toDissolve.AddComponent<DissolveController>();
            dissolve.setAndDissolveOut(dissolveGradient, dissolveTex);
        }
    }
}
