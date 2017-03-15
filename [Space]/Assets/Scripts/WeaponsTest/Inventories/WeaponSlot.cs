using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class WeaponSlot : MonoBehaviour
    {
        public GameObject weaponPrefab;
        private GameObject slotWeapon;
        public NVRInteractable weaponInt;
        private GameObject hoverWeapon;
        private NVRInteractable hoverInt;
        private Rigidbody weaponRB;
        private bool weaponInSlot;
        private WeaponSlotWrapper master;
        private bool hovering;
        private MeshRenderer currentMat;
        public Material defaultMat;
        public Material highlightMat;

        private void Start()
        {
            master = transform.parent.GetComponent<WeaponSlotWrapper>();
            currentMat = GetComponent<MeshRenderer>();
            hovering = false;
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
            else
                weaponInSlot = false;
        }

        private void Update()
        {
            if (hovering)
                currentMat.material = highlightMat;
            else if (currentMat.material != defaultMat)
                currentMat.material = defaultMat;

            if (!weaponInSlot)
            {
                if (hovering && hoverWeapon != null && !hoverInt.IsAttached)
                {
                    slotWeapon = hoverWeapon;
                    weaponInt = hoverInt;
                    weaponRB = hoverWeapon.GetComponent<Rigidbody>();
                    slotWeapon.transform.parent = transform;
                    slotWeapon.transform.localPosition = Vector3.zero;
                    slotWeapon.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                    weaponRB.useGravity = false;
                    weaponRB.isKinematic = true;
                    weaponInSlot = true;
                }
            }
            else if (weaponInt.IsAttached)
            {
                slotWeapon.transform.parent = null;
                weaponInSlot = false;
                master.toggleSlots();
            }
            hovering = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!weaponInSlot && other.transform.root.tag.Equals("Weapon") && !other.isTrigger)
            {
                NVRInteractable newInt = other.transform.root.GetComponent<NVRInteractable>();
                if (newInt != null && newInt.IsAttached)
                {
                    hoverWeapon = other.transform.root.gameObject;
                    hoverInt = newInt;
                    hovering = true;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!weaponInSlot)
            {
                if (other.transform.root.gameObject == hoverWeapon && !other.isTrigger)
                    hovering = true;
            }
        }
    }
}
