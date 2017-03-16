using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(LineRenderer))]
    public class Dash2 : MonoBehaviour
    {
        LineRenderer line;
        NVRPlayer player;
        NVRHand hand;
        NVRButtonInputs touchpad;
        RaycastHit hitInfo;

        Vector3 dashDirection = new Vector3(0, 0, 0);
        Vector3 startPoint;
        Vector3 endPoint;
        Vector3 leftInteractableOffset;
        Vector3 rightInteractableOffset;

        public float dashSpeed = 80.0f;
        public float dashDistance = 6.0f;
        private float distance;
        public float dashCooldown = 0.5f;
        private float cooldown;
        private float startTime;
        private float duration;

        public bool leftHanded = false;
        public bool gazeControl = false;
        public bool isDashing;

        void Start()
        {
            player = GetComponent<NVRPlayer>();
            line = GetComponent<LineRenderer>();
            line.enabled = false;
            line.numCapVertices = 4;
            line.numPositions = 2;
            NVRHelpers.LineRendererSetWidth(line, 0.2f, 0.2f);
            cooldown = 0.0f;
            distance = 0.0f;
            isDashing = false;

            if (leftHanded)
                hand = player.LeftHand;
            else
                hand = player.RightHand;

            touchpad = hand.Inputs[NVRButtons.Touchpad];
        }

        void Update()
        {
            if (cooldown > 0.0f)
                cooldown -= Time.deltaTime;
            else if (isDashing)
                dash();
            else if (touchpad.IsPressed && touchpad.Axis.y > 0.1f)
                startDash();
            else if (touchpad.IsTouched && touchpad.Axis.y > 0.1f)
                drawDashDirection();
            else
                line.enabled = false;
        }

        void drawDashDirection()
        {
            startPoint = player.Head.transform.position;
            startPoint.y = player.transform.position.y;
            distance = dashDistance * touchpad.Axis.y;

            if (gazeControl)
            {
                dashDirection.x = player.Head.transform.forward.x;
                dashDirection.z = player.Head.transform.forward.z;
            }
            else
            {
                dashDirection.x = hand.transform.forward.x;
                dashDirection.z = hand.transform.forward.z;
            }
            dashDirection = Vector3.Normalize(dashDirection);

            if (Physics.Raycast(startPoint, dashDirection, out hitInfo, distance))
                endPoint = hitInfo.point;
            else
                endPoint = startPoint + dashDirection * distance;

            line.SetPositions(new Vector3[] { startPoint, endPoint });
            line.enabled = true;
        }

        void startDash()
        {
            line.enabled = false;

            if (player.LeftHand.CurrentlyInteracting != null)
            {
                player.LeftHand.CurrentlyInteracting.GetComponent<Rigidbody>().isKinematic = true;
                leftInteractableOffset = player.LeftHand.CurrentlyInteracting.transform.position - player.LeftHand.transform.position;
            }
            if (player.RightHand.CurrentlyInteracting != null)
            {
                player.RightHand.CurrentlyInteracting.GetComponent<Rigidbody>().isKinematic = true;
                rightInteractableOffset = player.RightHand.CurrentlyInteracting.transform.position - player.RightHand.transform.position;
            }

            startTime = Time.time;
            duration = distance / dashSpeed;

            isDashing = true;
            dash();
        }

        void dash()
        {
            float t = (Time.time - startTime)/duration;
            if (t < 1)
            {
                player.transform.position = Vector3.Lerp(startPoint, endPoint, t);

                interactableCatchUp();
            }
            else
            {
                player.transform.position = endPoint;
                endDash();
            }
        }

        void endDash()
        {
            interactableCatchUp();

            if (player.LeftHand.CurrentlyInteracting != null)
                player.LeftHand.CurrentlyInteracting.GetComponent<Rigidbody>().isKinematic = false;
            if (player.RightHand.CurrentlyInteracting != null)
                player.RightHand.CurrentlyInteracting.GetComponent<Rigidbody>().isKinematic = false;

            isDashing = false;
            cooldown = dashCooldown;
        }

        void interactableCatchUp()
        {
            if (player.LeftHand.CurrentlyInteracting != null)
                player.LeftHand.CurrentlyInteracting.transform.position = player.LeftHand.transform.position + leftInteractableOffset;
            if (player.RightHand.CurrentlyInteracting != null)
                player.RightHand.CurrentlyInteracting.transform.position = player.RightHand.transform.position + rightInteractableOffset;
        }

    }
}
