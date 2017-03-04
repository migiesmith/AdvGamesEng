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

        // Use this for initialization
        void Start()
        {
            player = transform.root.GetComponent<NVRPlayer>();
            slot = GetComponent<NVRInteractableItem>();

            if (transform.name.Contains("Left"))
                offHand = player.RightHand;
            else if (transform.name.Contains("Right"))
                offHand = player.LeftHand;
            else
                Debug.Log("Invalid Setup: Check Naming Convention");

            updateSlotItem();
        }

        // Update is called once per frame
        void Update()
        {
            if (offHand.CurrentlyInteracting != equippedWeapon)
                updateSlotItem();        
        }

        void updateSlotItem()
        {
            if (offHand.CurrentlyInteracting != null && offHand.CurrentlyInteracting.tag.Equals("Reloadable"))
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
            }
            else if(slotItem != null)
                clearSlotItem();
        }

        void clearSlotItem()
        {
            slotItem = null;
            Destroy(itemDisplay.gameObject);
            itemDisplay = null;
        }

        public virtual void spawnConsumable()
        {
            if (slotItem != null)
            {
                hand = slot.AttachedHand;
                slot.ForceDetach();
                itemClone = Instantiate(slotItem, transform.position, transform.rotation).GetComponent<NVRInteractableItem>();
                itemClone.AttachedHand = hand;
                hand.CurrentlyInteracting = itemClone;
            }
        }
    }
}
