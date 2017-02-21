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

        // Use this for initialization
        void Start() {
            slot = this.GetComponent<NVRInteractableItem>();

            itemDisplay = Instantiate(slotItem, this.transform.position, this.transform.rotation);
            itemDisplay.transform.parent = this.transform;
            itemDisplay.GetComponent<Rigidbody>().isKinematic = true;
            itemDisplay.GetComponent<Rigidbody>().useGravity = false;
            itemDisplay.GetComponent<Collider>().enabled = false;
            itemDisplay.GetComponent<NVRInteractableItem>().enabled = false;
        }

        // Update is called once per frame
        void Update() {

        }

        public virtual void spawnConsumable()
        {
            hand = slot.AttachedHand;
            slot.ForceDetach();
            itemClone = Instantiate(slotItem, this.transform.position, this.transform.rotation).GetComponent<NVRInteractableItem>();
            itemClone.AttachedHand = hand;
            hand.CurrentlyInteracting = itemClone;
        }
    }
}
