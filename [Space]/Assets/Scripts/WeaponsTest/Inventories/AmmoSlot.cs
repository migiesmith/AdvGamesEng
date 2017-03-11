using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{

    public class AmmoSlot : MonoBehaviour
    {
        private NVRInteractableItem slot;
        private GameObject slotItem;
        private GameObject itemDisplay;
        private NVRInteractableItem itemClone;
        private NVRPlayer player;
        private NVRHand hand;
        private NVRHand offHand;
        private NVRInteractable equippedWeapon;
        private ConsumableInventory inventory;
        private TextMesh readout;

        private bool inInventory;
        public bool infinite = false;

        // Use this for initialization
        void Start()
        {
            player = transform.root.GetComponent<NVRPlayer>();
            slot = GetComponent<NVRInteractableItem>();
            readout = GetComponentInChildren<TextMesh>();
            readout.text = "";

            if (transform.name.Contains("Left"))
                offHand = player.RightHand;
            else if (transform.name.Contains("Right"))
                offHand = player.LeftHand;
            else
                Debug.Log("Invalid Setup: Check Naming Convention");

            updateSlotItem();

            if (transform.parent.GetComponent<ConsumableInventory>() != null)
                inventory = transform.parent.GetComponent<ConsumableInventory>();

            inInventory = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (offHand.CurrentlyInteracting != equippedWeapon)
                updateSlotItem();

            if (slotItem != null)
            {
                if (!inInventory && inventory.inventoryList[slotItem.name] > 0)
                {
                    inInventory = true;
                    itemDisplay.GetComponent<Renderer>().material.color = Color.white;
                    readout.text = inventory.inventoryList[slotItem.name].ToString();
                    readout.color = Color.white;
                }
                else if (inInventory && inventory.inventoryList[slotItem.name] <= 0)
                {
                    inInventory = false;
                    itemDisplay.GetComponent<Renderer>().material.color = Color.red;
                    readout.text = inventory.inventoryList[slotItem.name].ToString();
                    readout.color = Color.red;
                }
            }
        }

        void updateSlotItem()
        {
            if (offHand.CurrentlyInteracting != null && offHand.CurrentlyInteracting.GetComponent<Reloadable>() != null)
            {
                equippedWeapon = offHand.CurrentlyInteracting;

                slotItem = (GameObject)Resources.Load("Prefabs/Ammo/" + equippedWeapon.transform.name + "_Magazine");
                if (slotItem != null)
                {
                    itemDisplay = Instantiate(slotItem, transform.position, transform.rotation);
                    itemDisplay.transform.parent = transform;
                    if (itemDisplay.GetComponent<Rigidbody>() != null)
                    {
                        itemDisplay.GetComponent<Rigidbody>().isKinematic = true;
                        itemDisplay.GetComponent<Rigidbody>().useGravity = false;
                    }
                    if (itemDisplay.GetComponent<Collider>() != null)
                        itemDisplay.GetComponent<Collider>().enabled = false;
                    if (itemDisplay.GetComponent<NVRInteractableItem>() != null)
                        itemDisplay.GetComponent<NVRInteractableItem>().enabled = false;
                }
                readout.text = inventory.inventoryList[slotItem.name].ToString();
                if (inventory.inventoryList[slotItem.name] <= 0)
                {
                    inInventory = false;
                    itemDisplay.GetComponent<Renderer>().material.color = Color.red;
                    readout.color = Color.red;
                }
            }
            else if (slotItem != null)
            {
                equippedWeapon = null;
                clearSlotItem();
            }
        }

        void clearSlotItem()
        {
            slotItem = null;
            Destroy(itemDisplay.gameObject);
            itemDisplay = null;
            readout.text = "";
            readout.color = Color.white;
        }

        public virtual void spawnConsumable()
        {
            if (slotItem != null && inventory.inventoryList[slotItem.name] > 0)
            {
                hand = slot.AttachedHand;
                slot.ForceDetach();
                itemClone = Instantiate(slotItem, transform.position, transform.rotation).GetComponent<NVRInteractableItem>();
                itemClone.AttachedHand = hand;
                hand.CurrentlyInteracting = itemClone;
                if (!infinite)
                {
                    --inventory.inventoryList[slotItem.name];
                    readout.text = inventory.inventoryList[slotItem.name].ToString();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (slotItem != null)
            {
                if (other.transform.name.Contains(slotItem.name) && other.GetComponent<NVRInteractableItem>().AttachedHand == null)
                {
                    if (!infinite)
                    {
                        ++inventory.inventoryList[slotItem.name];
                        readout.text = inventory.inventoryList[slotItem.name].ToString();
                    }
                    other.enabled = false;
                    Destroy(other.gameObject);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (slotItem != null)
            {
                if (other.transform.name.Contains(slotItem.name) && other.GetComponent<NVRInteractableItem>().AttachedHand == null)
                {
                    if (!infinite)
                    {
                        ++inventory.inventoryList[slotItem.name];
                        readout.text = inventory.inventoryList[slotItem.name].ToString();
                    }
                    other.enabled = false;
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
