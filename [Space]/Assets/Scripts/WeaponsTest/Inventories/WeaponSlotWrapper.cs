using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class WeaponSlotWrapper : MonoBehaviour
    {
        private NVRPlayer player;
        private WeaponSlot[] slots;
        public NVRButtons activationInput = NVRButtons.ApplicationMenu;
        private NVRButtonInputs leftActivate;
        private NVRButtonInputs rightActivate;
        private bool isVisible;
        // Use this for initialization
        void Start()
        {
            player = transform.root.GetComponent<NVRPlayer>();
            slots = GetComponentsInChildren<WeaponSlot>();
            foreach (WeaponSlot slot in slots)
                slot.gameObject.SetActive(false);
            leftActivate = player.LeftHand.Inputs[activationInput];
            rightActivate = player.RightHand.Inputs[activationInput];
            isVisible = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (leftActivate.PressDown || rightActivate.PressDown)
                toggleSlots();
        }

        public void toggleSlots()
        {
            if (isVisible)
                foreach (WeaponSlot slot in slots)
                {
                    slot.gameObject.SetActive(false);
                    if (slot.weaponInt != null)
                    {
                        if (player.LeftHand.CurrentlyHoveringOver.ContainsKey(slot.weaponInt))
                            player.LeftHand.CurrentlyHoveringOver.Remove(slot.weaponInt);
                        if (player.RightHand.CurrentlyHoveringOver.ContainsKey(slot.weaponInt))
                            player.RightHand.CurrentlyHoveringOver.Remove(slot.weaponInt);
                    }
                }
            else
                foreach (WeaponSlot slot in slots)
                    slot.gameObject.SetActive(true);
            isVisible = !isVisible;
        }
    }
}
