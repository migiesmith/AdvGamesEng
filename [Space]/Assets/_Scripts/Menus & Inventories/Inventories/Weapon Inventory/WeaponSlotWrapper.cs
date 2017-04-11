using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class WeaponSlotWrapper : MonoBehaviour
    {
        private NVRPlayer player;
        public PrefabDatabase prefabs;
        private WeaponSlot[] slots;
        public NVRButtons activationInput = NVRButtons.ApplicationMenu;
        private NVRButtonInputs leftActivate;
        private NVRButtonInputs rightActivate;
        private bool isVisible;
        public bool locked;
        // Use this for initialization
        void Start()
        {
            player = transform.root.GetComponent<NVRPlayer>();
            slots = GetComponentsInChildren<WeaponSlot>();
            leftActivate = player.LeftHand.Inputs[activationInput];
            rightActivate = player.RightHand.Inputs[activationInput];
            foreach (WeaponSlot slot in slots)
            {
                slot.initialise();
                slot.gameObject.SetActive(false);
            }
            isVisible = false;
            locked = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!locked && (leftActivate.PressDown || rightActivate.PressDown))
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


        public List<string> setHeldWeapons()
        {
            List<string> send = new List<string>();

            foreach (var slot in slots)
            {
                if (slot.weaponPrefab != null)
                    send.Add(slot.weaponPrefab.name);
                else
                    send.Add("Empty");
            }
            return send;
        }

        public void getHeldWeapons(List<string> heldWeapons)
        {
            int slotNumber = 0;
            foreach(string name in heldWeapons)
            {
                if (heldWeapons[slotNumber] != null && heldWeapons[slotNumber] != "Empty")
                {
                    GameObject prefab = prefabs.getPrefab(heldWeapons[slotNumber]);
                    if (prefab != null)
                    {
                        slots[slotNumber].weaponPrefab = prefab;
                        slots[slotNumber].initialise();
                    }
                }               
                ++slotNumber;
            }
        }
    }
}
