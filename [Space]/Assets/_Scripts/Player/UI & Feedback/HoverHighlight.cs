using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class HoverHighlight : MonoBehaviour
    {
        private List<GameObject> swapped = new List<GameObject>();
        private NVRHand hand;
        private Outline outline;

        void Start()
        {
            outline = GetComponentInParent<Outline>();
        }

        public void clearHighlight(GameObject item)
        {
            if (swapped.Contains(item))
            {
                outline.hide(item);
                swapped.Remove(item);
            }
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (NVRInteractables.GetInteractable(other) != null && !swapped.Contains(other.gameObject) && other.GetComponent<ConsumableSlot>() == null && other.GetComponent<AmmoSlot>() == null)
            {
                outline.show(other.gameObject);
                swapped.Add(other.gameObject);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (NVRInteractables.GetInteractable(other) != null && !swapped.Contains(other.gameObject) && other.GetComponent<ConsumableSlot>() == null && other.GetComponent<AmmoSlot>() == null)
            {
                outline.show(other.gameObject);
                swapped.Add(other.gameObject);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (swapped.Contains(other.gameObject))
            {
                outline.hide(other.gameObject);
                swapped.Remove(other.gameObject);
            }
        }
    }
}
