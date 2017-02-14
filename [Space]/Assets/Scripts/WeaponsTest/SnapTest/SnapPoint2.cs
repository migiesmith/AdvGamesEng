using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class SnapPoint2 : MonoBehaviour
    {
        NVRInteractableItem snapObject = null;
        NVRInteractableItem triggerObject = null;
        public Material defaultMat;
        public Material highlightMat;
        private MeshRenderer currentMat;
        private bool isSnapped;

        private void Start()
        {
            currentMat = this.GetComponent<MeshRenderer>();
        }

        void Update()
        {
            if (isSnapped && snapObject != null)
            {
                if (snapObject.AttachedHand != null)
                {
                    isSnapped = false;
                    snapObject = null;
                }
                else
                {
                    snapObject.transform.position = this.transform.position;
                    snapObject.transform.localRotation = this.transform.rotation;
                }
            }
        }
    
        private void OnTriggerEnter(Collider snapZone)
        {
            triggerObject = snapZone.gameObject.GetComponent<NVRInteractableItem>();
            snapCheck();
        }

        private void OnTriggerStay(Collider snapZone)
        {
            if (triggerObject == null)
                triggerObject = snapZone.gameObject.GetComponent<NVRInteractableItem>();
            snapCheck();
        }

        private void OnTriggerExit(Collider snapZone)
        {
            resetHighlight();
        }

        void snapCheck()
        {
            if (!isSnapped && snapObject == null && triggerObject != null && triggerObject.enabled == true)
            {
                if (triggerObject.AttachedHand != null)
                    highlightSnap();
                else
                {
                    isSnapped = true;
                    snapObject = triggerObject;
                    resetHighlight();
                }
            }
        }

        void highlightSnap()
        {
            if (currentMat.material != highlightMat)
                currentMat.material = highlightMat;
        }

        void resetHighlight()
        {
            if (currentMat.material != defaultMat)
                currentMat.material = defaultMat;
        }
    }
}
