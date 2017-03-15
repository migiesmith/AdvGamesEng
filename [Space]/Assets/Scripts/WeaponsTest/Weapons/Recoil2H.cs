using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Recoil2H : MonoBehaviour
    {
        private TwoHandedInteractableItem gun;
        private Transform primaryInteractionPoint;
        private Transform secondaryInteractionPoint;

        public float riseTime = 0.01f;
        public float fallTime = 0.05f;

        public float oneHandRecoilDeflection = 30.0f;
        private float riseAngleDelta;
        private float fallAngleDelta;
        private Quaternion resetRotation;
        private Quaternion recoilRotation;

        public float twoHandRecoilDeflection = 15.0f;
        private float risePositionDelta;
        private float fallPositionDelta;
        private Vector3 resetPosition;
        private Vector3 recoilPosition;

        private bool twoHands;
        private bool inRecoil;
        private bool rising;
        private float timer;

        // Use this for initialization
        void Start()
        {
            gun = GetComponent<TwoHandedInteractableItem>();
            primaryInteractionPoint = gun.InteractionPoint;
            secondaryInteractionPoint = gun.SecondInteractionPoint;

            riseAngleDelta = oneHandRecoilDeflection / riseTime;
            fallAngleDelta = oneHandRecoilDeflection / fallAngleDelta;
            resetRotation = primaryInteractionPoint.localRotation;
            recoilRotation = primaryInteractionPoint.localRotation * Quaternion.AngleAxis(oneHandRecoilDeflection, primaryInteractionPoint.transform.right);

            resetPosition = secondaryInteractionPoint.localPosition;
            recoilPosition = resetPosition + Vector3.down * Vector3.Magnitude(secondaryInteractionPoint.localPosition - primaryInteractionPoint.localPosition) * Mathf.Sin(twoHandRecoilDeflection);
            risePositionDelta = Vector3.Magnitude(recoilPosition - resetPosition) / riseTime;
            fallPositionDelta = Vector3.Magnitude(resetPosition - recoilPosition) / fallTime;

            twoHands = false;
            inRecoil = false;
            rising = false;
            timer = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (!twoHands)
            {
                if (gun.SecondAttachedHand != null)
                {
                    twoHands = true;
                    primaryInteractionPoint.localRotation = resetRotation;
                }
            }
            else if (gun.SecondAttachedHand == null)
            {
                twoHands = false;
                secondaryInteractionPoint.localPosition = resetPosition;
            }

            if (inRecoil)
            {
                if (rising)
                {
                    if (!twoHands)
                        rise1H();
                    else
                        rise2H();
                }
                else
                {
                    if (!twoHands)
                        fall1H();
                    else
                        fall2H();
                }
            }
        }

        private void rise1H()
        {
            primaryInteractionPoint.localRotation = Quaternion.RotateTowards(resetRotation, recoilRotation, riseAngleDelta);
            if (primaryInteractionPoint.localRotation == recoilRotation)
                rising = false;
        }

        private void rise2H()
        {
            secondaryInteractionPoint.localPosition = Vector3.MoveTowards(secondaryInteractionPoint.localPosition, recoilPosition, risePositionDelta);
            if (secondaryInteractionPoint.localPosition == recoilPosition)
                rising = false;
        }

        private void fall1H()
        {
            primaryInteractionPoint.localRotation = Quaternion.RotateTowards(recoilRotation, resetRotation, fallAngleDelta);
            if (primaryInteractionPoint.localRotation == resetRotation)
                inRecoil = false;
        }

        private void fall2H()
        {
            secondaryInteractionPoint.localPosition = Vector3.MoveTowards(secondaryInteractionPoint.localPosition, resetPosition, fallPositionDelta);
            if (secondaryInteractionPoint.localPosition == resetPosition)
                inRecoil = false;
        }

        public void recoilStart()
        {
            inRecoil = true;
            rising = true;
            if (!twoHands)
                rise1H();
            else
                rise2H();
        }

        public void resetRecoil()
        {
            primaryInteractionPoint.localRotation = resetRotation;
            secondaryInteractionPoint.localPosition = resetPosition;
            inRecoil = false;
            rising = false;
            twoHands = false;
        }
    }
}

