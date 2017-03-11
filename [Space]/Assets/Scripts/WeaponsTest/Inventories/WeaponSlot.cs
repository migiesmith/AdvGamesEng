using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class WeaponSlot : MonoBehaviour {
        public GameObject weaponPrefab;
        private GameObject slotWeapon;
        public NVRInteractable weaponInt;
        private Rigidbody weaponRB;
        private NVRPlayer player;
        public bool weaponInSlot;
        private WeaponSlotWrapper master;

        private void Start()
        {
            player = transform.root.GetComponent<NVRPlayer>();
            master = transform.parent.GetComponent<WeaponSlotWrapper>();
            if (weaponPrefab != null)
            {
                weaponInSlot = true;
                slotWeapon = Instantiate(weaponPrefab, transform);
                slotWeapon.name = weaponPrefab.name;
                slotWeapon.transform.localPosition = Vector3.zero;
                slotWeapon.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                weaponInt = slotWeapon.GetComponent<NVRInteractable>();
                weaponRB = slotWeapon.GetComponent<Rigidbody>();
                weaponRB.useGravity = false;
                weaponRB.isKinematic = true;
            }
        }

        private void Update()
        {
            if (weaponInSlot && (player.LeftHand.CurrentlyInteracting == weaponInt || player.RightHand.CurrentlyInteracting == weaponInt))
            {
                slotWeapon.transform.parent = null;
                weaponInSlot = false;
                master.toggleSlots();
            }
        }

        private void addWeapon(Collider other)
        {
            if (!weaponInSlot)
            {
                if (other.transform.root.gameObject == slotWeapon)
                {
                    if (player.LeftHand.CurrentlyInteracting != weaponInt && player.RightHand.CurrentlyInteracting != weaponInt)
                    {
                        slotWeapon.transform.parent = transform;
                        slotWeapon.transform.localPosition = Vector3.zero;
                        slotWeapon.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                        weaponRB.useGravity = false;
                        weaponRB.isKinematic = true;
                        weaponInSlot = true;
                    }
                }
                else if (other.transform.root.tag.Equals("Weapon"))
                {
                    NVRInteractable newInt = other.transform.root.GetComponent<NVRInteractable>();
                    if (newInt != null)
                    {
                        if (player.LeftHand.CurrentlyInteracting != newInt && player.RightHand.CurrentlyInteracting != newInt)
                        {
                            slotWeapon = other.transform.root.gameObject;
                            weaponInt = newInt;
                            weaponRB = slotWeapon.GetComponent<Rigidbody>();
                            slotWeapon.transform.parent = transform;
                            slotWeapon.transform.localPosition = Vector3.zero;
                            slotWeapon.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                            weaponRB.useGravity = false;
                            weaponRB.isKinematic = true;
                            weaponInSlot = true;
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            addWeapon(other);
        }

        private void OnTriggerStay(Collider other)
        {
            addWeapon(other);
        }
    }
}
