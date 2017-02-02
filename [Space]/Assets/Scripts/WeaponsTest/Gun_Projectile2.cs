using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractableItem))]
    public class Gun_Projectile2 : MonoBehaviour
    {
        public NVRInteractableItem gun;
        public GameObject bulletPrefab;
        public Transform muzzle;
        public Transform magwell;
        private GameObject magazine;

        public Vector3 power = new Vector3(0, 0, 250);
        public float Refire = 0.2f;
        private float RefireDelay = 0.0f;
        public float bulletLifetime = 5.0f;
        public int AmmoCapacity = 12;
        private int AmmoCount;

        private void Start()
        {
            AmmoCount = 0;
        }

        void Update()
        {
            magazine = gun.transform.FindChild("CURR_MAG").gameObject;
            if (magazine != null)
                magazine.gameObject.transform.position = magwell.transform.position;

            if (gun.AttachedHand.UseButtonDown && RefireDelay <= 0)
            {
                if (AmmoCount > 0)
                {
                    GameObject bullet = Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation) as GameObject;
                    bullet.SetActive(true);
                    bullet.GetComponent<Rigidbody>().AddRelativeForce(power);
                    Destroy(bullet, bulletLifetime);

                    gun.AttachedHand.TriggerHapticPulse(1000);
                    RefireDelay = Refire;
                    --AmmoCount;
                }
                else
                {
                    if (magazine != null)
                    {
                        magazine.transform.gameObject.name = "Empty";
                        magazine.transform.parent = null;
                        magazine.GetComponent<Rigidbody>().useGravity = true;
                        magazine.GetComponent<Rigidbody>().isKinematic = false;
                        magazine.GetComponent<Collider>().enabled = true;
                        Destroy(magazine.gameObject, 10.0f);
                        magazine = null;
                    }
                }
            }
            else if (RefireDelay > 0)
                RefireDelay -= Time.deltaTime;
        }

        private void OnTriggerEnter(Collider magdetect)
        {
            if (magdetect.gameObject.name.Contains("Magazine") && magazine == null)
            {
                AmmoCount = AmmoCapacity;

                magdetect.gameObject.transform.parent = gun.transform;
                magdetect.gameObject.transform.position = magwell.transform.position;
                magdetect.gameObject.transform.rotation = magwell.transform.rotation;
                magdetect.gameObject.transform.localScale = magwell.transform.localScale;

                magdetect.gameObject.GetComponent<Rigidbody>().useGravity = false;
                magdetect.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                magdetect.gameObject.GetComponent<Collider>().enabled = false;

                magdetect.gameObject.GetComponent<NVRInteractableItem>().AttachedHand = null;
                magdetect.gameObject.name = "CURR_MAG";
            }
        }
    }
}
