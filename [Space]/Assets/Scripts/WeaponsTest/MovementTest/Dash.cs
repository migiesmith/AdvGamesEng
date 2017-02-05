using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(LineRenderer))]
    public class Dash : MonoBehaviour
    {
        public NVRPlayer player;
        private LineRenderer line;

        public float dashSpeed = 80.0f;
        public float dashCooldown = 0.0f;
        public float dashDuration = 0.1f;
        private float duration;
        private float cooldown;
        private bool isDashing = false;
        public float yOffset = 0.01f;

        private Vector3 dashDirection = new Vector3(0, 0, 0);
        private Vector3 endPoint = new Vector3(0, 0, 0);
        RaycastHit hitInfo;

        public bool gazeControl = false;

        private void Start()
        {
            line = GetComponent<LineRenderer>();
            line.enabled = false;
            line.numCapVertices = 4;
            line.numPositions = 2;
            NVRHelpers.LineRendererSetWidth(line, 0.2f, 0.2f);
            cooldown = 0.0f;
        }

        void Update()
        {
            line.enabled = false;

            if (player.RightHand.Inputs[NVRButtons.Touchpad].IsPressed && player.RightHand.Inputs[NVRButtons.Touchpad].Axis.y > 0.1f && cooldown <= 0.0f && !isDashing)
            {
                startDash();
            }
            else if (player.RightHand.Inputs[NewtonVR.NVRButtons.Touchpad].IsTouched && player.RightHand.Inputs[NVRButtons.Touchpad].Axis.y > 0.1f && cooldown <= 0.0f && !isDashing)
            {
                drawDashDirection();
            }
            else if (isDashing)
            {
                dash();
            }
            else if (cooldown > 0.0f)
            {
                cooldown -= Time.deltaTime;
            }
        }

        void startDash()
        {
            line.enabled = false;
            isDashing = true;

            if (player.LeftHand.CurrentlyInteracting != null)
                player.LeftHand.CurrentlyInteracting.GetComponent<Rigidbody>().isKinematic = true;

            if (player.RightHand.CurrentlyInteracting != null)
                player.RightHand.CurrentlyInteracting.GetComponent<Rigidbody>().isKinematic = true;
                
            dash();
        }

        void dash()
        {
            if (Vector3.Distance(player.transform.position, endPoint) > dashSpeed * Time.deltaTime)
            {
                Vector3 tickStep = dashDirection * dashSpeed * Time.deltaTime;
                player.transform.Translate(tickStep);
                interactableCatchUp();
            }
            else
            {
                player.transform.position = endPoint;
                interactableCatchUp();
                endDash();
            }
        }

        protected void drawDashDirection()
        {
            if (!gazeControl)
            {
                dashDirection.x = player.RightHand.transform.forward.x;
                dashDirection.z = player.RightHand.transform.forward.z;
            }
            else
            {
                dashDirection.x = player.Head.transform.forward.x;
                dashDirection.z = player.Head.transform.forward.z;
            }

            dashDirection = Vector3.Normalize(dashDirection);
            line.enabled = true;
            duration = dashDuration * player.RightHand.Inputs[NVRButtons.Touchpad].Axis.y;

            if (Physics.Raycast(new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z), dashDirection, out hitInfo, dashSpeed * duration))
                endPoint = new Vector3(hitInfo.point.x, player.transform.position.y, hitInfo.point.z);
            else
                endPoint = player.transform.position + dashDirection * dashSpeed * duration;

            line.SetPositions(new Vector3[] { player.transform.position, endPoint});
        }

        void endDash()
        {
            cooldown = dashCooldown;
            isDashing = false;

            if (player.LeftHand.CurrentlyInteracting != null)
            {
                player.LeftHand.CurrentlyInteracting.GetComponent<Rigidbody>().isKinematic = false;
                player.LeftHand.CurrentlyInteracting.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }

            if (player.RightHand.CurrentlyInteracting != null)
            {
                player.RightHand.CurrentlyInteracting.GetComponent<Rigidbody>().isKinematic = false;
                player.RightHand.CurrentlyInteracting.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
        }

        void interactableCatchUp()
        {
            if (player.LeftHand.CurrentlyInteracting != null)
                player.LeftHand.CurrentlyInteracting.transform.position = player.LeftHand.transform.position;

            if (player.RightHand.CurrentlyInteracting != null)
                player.RightHand.CurrentlyInteracting.transform.position = player.RightHand.transform.position;
        }
    }
}
