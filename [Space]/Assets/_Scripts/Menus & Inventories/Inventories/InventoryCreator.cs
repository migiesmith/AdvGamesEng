using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class InventoryCreator : MonoBehaviour
    {
        public InventoryMenu menu;

        NVRPlayer player;
        NVRButtonInputs touchpad;
        public bool leftHanded = false;

        bool menuOpen = false;

        public PlayerState state;
        public float cooldown;

        private void Start()
        {
            menu.gameObject.SetActive(false);

            player = GetComponent<NVRPlayer>();
            //Inventory is opened using left hand.
            touchpad = player.LeftHand.Inputs[NVRButtons.Touchpad];
        }


        private void Update()
        {
            if (cooldown > 0.0f)
                cooldown -= Time.deltaTime;

            //Opens and Closes the Inventory menu. Button prompt is opposite of dash.
            if (menuOpen && touchpad.Axis.x > 0.1f && touchpad.PressDown)
            {
                menuOpen = false;
                menu.gameObject.SetActive(false);
            }
            else if (!menuOpen && touchpad.Axis.x > 0.1f && touchpad.PressDown && cooldown <= 0.0f)
            {
                menuOpen = true;
                menu.gameObject.SetActive(true);
            }

            if (state.isInCombat())
            {
                cooldown = 2.0f;
            }
        }
    }
}
