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

        void checkHighlight(Collider toCheck)
        {
            if (!toCheck.isTrigger)
            {
                NVRInteractable checkInt = NVRInteractables.GetInteractable(toCheck);
                if (checkInt != null)
                {
                    if (checkInt.IsAttached && swapped.Contains(toCheck.gameObject))
                        clearHighlight(toCheck.gameObject);
                    else if (!checkInt.IsAttached && !swapped.Contains(toCheck.gameObject))
                    {
                        outline.show(toCheck.gameObject);
                        swapped.Add(toCheck.gameObject);
                    }
                }
            }
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
            checkHighlight(other);
        }

        void OnTriggerStay(Collider other)
        {
            checkHighlight(other);
        }

        void OnTriggerExit(Collider other)
        {
            clearHighlight(other.gameObject);
        }
    }
}
