using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class HoverHighlight : MonoBehaviour
    {
        private Dictionary<GameObject, Material> defaultMat = new Dictionary<GameObject, Material>();
        public Material highlightMat;
        private NVRHand hand;

        // Use this for initialization
        void Start()
        {
            hand = GetComponent<NVRHand>();
        }

        // Update is called once per frame
        void Update()
        {
            if (hand.IsHovering && !hand.IsInteracting)
                foreach(NVRInteractable item in hand.CurrentlyHoveringOver.Keys)
                {
                    MeshRenderer mr = item.GetComponentInChildren<MeshRenderer>();
                    if (!defaultMat.ContainsKey(item.gameObject) && mr.material != highlightMat && !item.IsAttached)
                    {
                        defaultMat.Add(item.gameObject, mr.material);
                        mr.material = highlightMat;
                    }

                }
            else
            {
                foreach(GameObject item in defaultMat.Keys)
                {
                    MeshRenderer mr = item.GetComponentInChildren<MeshRenderer>();
                    mr.material = defaultMat[item];
                }
                defaultMat.Clear();
            }
        }
    }
}
