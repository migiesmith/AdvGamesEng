using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Recoil1H : MonoBehaviour
    {
        private NVRInteractableItem gun;
        private Transform interactionPoint;

        public float recoilDeflection = 30.0f;
        public float riseTime = 0.01f;
        private float riseDelta;
        public float fallTime = 0.05f;
        private float fallDelta;

        private Quaternion resetRotation;
        private Quaternion recoilRotation;
        private bool inRecoil;
        private bool rising;

        // Use this for initialization
        void Start()
        {
            gun = GetComponent<NVRInteractableItem>();
            interactionPoint = gun.InteractionPoint;

            riseDelta = recoilDeflection / riseTime;
            fallDelta = recoilDeflection / fallDelta;

            resetRotation = interactionPoint.localRotation;
            recoilRotation = interactionPoint.localRotation * Quaternion.AngleAxis(recoilDeflection, interactionPoint.transform.right);
            inRecoil = false;
            rising = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (inRecoil)
            {
                if (rising)
                    rise();
                else
                    fall();
            }
        }

        private void rise()
        {
            interactionPoint.localRotation = Quaternion.RotateTowards(resetRotation, recoilRotation, riseDelta);
            if (interactionPoint.localRotation == recoilRotation)
                rising = false;
        }

        private void fall()
        {
            interactionPoint.localRotation = Quaternion.RotateTowards(recoilRotation, resetRotation, fallDelta);
            if (interactionPoint.localRotation == resetRotation)
                inRecoil = false;
        }

        public void recoilStart()
        {
            inRecoil = true;
            rising = true;
            rise();
        }

        public void resetRecoil()
        {
            if (inRecoil)
            {
                inRecoil = false;
                rising = false;
                interactionPoint.localRotation = resetRotation;
            }
        }
    }
}
