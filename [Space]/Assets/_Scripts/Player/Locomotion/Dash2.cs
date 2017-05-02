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

        Vector3 dashDirection = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 headStart;
        Vector3 headEnd;
        Vector3 playerStart;
        Vector3 playerEnd;
        Vector3 lineOffset = new Vector3(0.0f, 0.05f, 0.0f);

        public float borderWidth = 0.5f;
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
        private int inCombat;

        public PlayerState state;
        public int combatLimit = 3;
        public float combatCooldown = 2.0f;
        private int dashCount;
        private float limitTimer;

        void Start()
        {
            player = GetComponent<NVRPlayer>();
            line = GetComponent<LineRenderer>();
            line.enabled = false;
            line.numCapVertices = 4;
            line.numPositions = 2;
            NVRHelpers.LineRendererSetWidth(line, 0.1f, 0.1f);
            cooldown = 0.0f;
            distance = 0.0f;
            isDashing = false;

            /*if (leftHanded)
                hand = player.LeftHand;
            else
                hand = player.RightHand;*/

            hand = player.RightHand; //Dashing is with right hand, inventory with left.

            touchpad = hand.Inputs[NVRButtons.Touchpad];
            dashCount = 0;
            limitTimer = 0.0f;
        }

        void Update()
        {
            if (cooldown > 0.0f)
                cooldown -= Time.deltaTime;

            if (limitTimer > 0.0f)
                limitTimer -= Time.deltaTime;
            else
                dashCount = 0;

            if (isDashing)
                dash();
            else if (touchpad.Axis.y > 0.1f)
            {
                drawDashDirection();
                if (touchpad.IsPressed && cooldown <= 0.0f)
                    startDash();
            }
            else
                line.enabled = false;
        }

        void drawDashDirection()
        {
            headStart = player.Head.transform.position;
            headStart.y = player.transform.position.y;
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

            if (Physics.Raycast(headStart, dashDirection.normalized, out hitInfo))
            {
                Plane edgeBorder = new Plane(hitInfo.normal, hitInfo.point + borderWidth*hitInfo.normal);
                float borderHit;
                if (edgeBorder.Raycast(new Ray(headStart, dashDirection), out borderHit))
                {
                    if (borderHit < distance)
                        headEnd = headStart + dashDirection.normalized * borderHit;
                    else
                        headEnd = headStart + dashDirection.normalized * distance;
                }
            }
            else
                headEnd = headStart + dashDirection.normalized * distance;

            line.SetPositions(new Vector3[] { headStart + lineOffset, headEnd + lineOffset });
            line.enabled = true;
        }

        void startDash()
        {
            playerStart = player.transform.position;
            playerEnd = playerStart + (headEnd - headStart);

            line.enabled = false;

            if (player.LeftHand.CurrentlyInteracting != null)
                player.LeftHand.CurrentlyInteracting.transform.parent = player.LeftHand.transform;

            if (player.RightHand.CurrentlyInteracting != null)
                player.RightHand.CurrentlyInteracting.transform.parent = player.RightHand.transform;

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
                player.transform.position = Vector3.Lerp(playerStart, playerEnd, t);
            }
            else
            {
                player.transform.position = playerEnd;
                endDash();
            }
        }

        void endDash()
        {
            if (player.LeftHand.CurrentlyInteracting != null)
                player.LeftHand.CurrentlyInteracting.transform.parent = null;

            if (player.RightHand.CurrentlyInteracting != null)
                player.RightHand.CurrentlyInteracting.transform.parent = null;

            isDashing = false;
            cooldown = dashCooldown;

            if (state.isInCombat())
            {
                if (dashCount >= 2)
                {
                    cooldown = combatCooldown;
                    dashCount = 0;
                }
                else
                {
                    limitTimer = combatCooldown;
                    ++dashCount;
                }
            }
        }
    }
}
