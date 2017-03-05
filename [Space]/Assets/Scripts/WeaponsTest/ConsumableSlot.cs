using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{

    public class ConsumableSlot : MonoBehaviour {

        private NVRInteractableItem slot;
        public GameObject slotItem;
        private GameObject itemDisplay;
        private NVRInteractableItem itemClone;
        private NVRHand hand;
        private ConsumableInventory inventory;
        private TextMesh readout;

        private bool inInventory;
        public bool infinite = false;

        // Use this for initialization
        void Start() {
            slot = this.GetComponent<NVRInteractableItem>();
            readout = GetComponentInChildren<TextMesh>();
            readout.text = "0";

            if (slotItem != null)
            {
                itemDisplay = Instantiate(slotItem, this.transform.position, this.transform.rotation);
                itemDisplay.transform.parent = this.transform;
                itemDisplay.GetComponent<Rigidbody>().isKinematic = true;
                itemDisplay.GetComponent<Rigidbody>().useGravity = false;
                itemDisplay.GetComponent<Collider>().enabled = false;
                itemDisplay.GetComponent<NVRInteractableItem>().enabled = false;
            }
            if (transform.parent.GetComponent<ConsumableInventory>() != null)
                inventory = transform.parent.GetComponent<ConsumableInventory>();

            inInventory = false;
        }

        // Update is called once per frame
        void Update() {
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

        public virtual void spawnConsumable()
        {
            if (slotItem != null && inventory.inventoryList[slotItem.name] > 0)
            {
                hand = slot.AttachedHand;
                slot.ForceDetach();
                itemClone = Instantiate(slotItem, this.transform.position, this.transform.rotation).GetComponent<NVRInteractableItem>();
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
