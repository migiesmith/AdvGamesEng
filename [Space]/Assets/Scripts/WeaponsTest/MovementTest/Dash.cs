using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NewtonVR;

namespace space
{
    public class Dash : MonoBehaviour
    {
        public NVRPlayer Player;
        public SteamVR_TrackedController hand;
        public float dashSpeed = 100.0f;
        public float dashCooldown = 0.0f;
        public float dashDuration = 0.07f;
        private float cooldown;
        private float duration;
        private bool isDashing = false;
        private Vector3 dashDirection = new Vector3(0, 0, 0);

        public bool GazeControl = false;

        private void Start()
        {
            cooldown = 0.0f;
            duration = dashDuration;
        }

        void Update()
        {        
            if (hand.padPressed && cooldown <= 0.0f && !isDashing)
            {
                if (!GazeControl)
                {
                    dashDirection.x = Player.RightHand.transform.forward.x;
                    dashDirection.z = Player.RightHand.transform.forward.z;
                }
                else
                {
                    dashDirection.x = Player.Head.transform.forward.x;
                    dashDirection.z = Player.Head.transform.forward.z;
                }
                dash();
            }
            else if (isDashing)
            {
                if (duration <= 0.0f)
                    endDash();
                else
                    dash();
            }
            else if (cooldown > 0.0f)
                cooldown -= Time.deltaTime;
        }

        void dash()
        {
            NavMeshHit nav;
            if (NavMesh.SamplePosition(Player.transform.position + dashDirection * dashSpeed * Time.deltaTime, out nav, 0.1f, NavMesh.AllAreas))
            {
                Player.transform.position += dashDirection * dashSpeed * Time.deltaTime;
                duration -= Time.deltaTime;
                isDashing = true;

                if (Player.LeftHand.CurrentlyInteracting != null)
                    Player.LeftHand.CurrentlyInteracting.transform.position = Player.LeftHand.transform.position;

                if (Player.RightHand.CurrentlyInteracting != null)
                    Player.RightHand.CurrentlyInteracting.transform.position = Player.RightHand.transform.position;
            }
            else
                endDash();
        }

        void endDash()
        {
            duration = dashDuration;
            cooldown = dashCooldown;
            isDashing = false;

            if (Player.LeftHand.CurrentlyInteracting != null)
                Player.LeftHand.CurrentlyInteracting.transform.position = Player.LeftHand.transform.position;

            if (Player.RightHand.CurrentlyInteracting != null)
                Player.RightHand.CurrentlyInteracting.transform.position = Player.RightHand.transform.position;
        }
    }
}
