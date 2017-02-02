using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractableItem)), RequireComponent(typeof(LineRenderer))]
    public class Gun_Hitscan : MonoBehaviour
    {
        public NVRInteractableItem gun;
        public Transform muzzle;
        public Transform magwell;
        private GameObject magazine;

        public float power = 10;
        public float weaponDamage = 0.5f;
        public float refire = 0.2f;
        private float refireDelay = 0.0f;
        public int ammoCapacity = 12;
        private int ammoCount;

        private LineRenderer line;
        public Color lineColour = Color.cyan;
        public float lineWidth = 0.02f;

        private bool pulseActive = false;
        public float pulseDuration = 0.5f;

        RaycastHit hitInfo;
        Vector3 endPoint;
        GameObject target = null;

        private void Start()
        {
            line = this.GetComponent<LineRenderer>();
            ammoCount = 0;
            line.material.SetColor("_Color", lineColour);
            NVRHelpers.LineRendererSetColor(line, lineColour, lineColour);
            NVRHelpers.LineRendererSetWidth(line, lineWidth, lineWidth);
            line.enabled = false;
        }

        void Update()
        {
            magazine = gun.transform.FindChild("CURR_MAG").gameObject;
            if (magazine != null)
                magazine.gameObject.transform.position = magwell.transform.position;

            if (gun.AttachedHand.UseButtonDown && refireDelay <= 0)
            {
                if (ammoCount > 0)
                {
                    pulseDuration = 0.5f;
                    pulseActive = true;
                    fire();
                    line.enabled = true;
                    --ammoCount;
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
                    }
                }
            }
            else if (pulseActive && pulseDuration > 0)
            {
                fire();
                pulseDuration -= Time.deltaTime;
            }
            else if (pulseActive && pulseDuration <= 0)
            {
                refireDelay = 0.2f;
                pulseActive = false;
                line.enabled = false;
            }
            else if (!pulseActive && refireDelay > 0)
                refireDelay -= Time.deltaTime;
        }

        void fire()
        {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hitInfo, 1000))
            {
                endPoint = hitInfo.point;
                line.SetPositions(new Vector3[] { muzzle.transform.position, endPoint });
                target = hitInfo.transform.gameObject;

                if (target.GetComponent<Rigidbody>() != null)
                    target.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * power);

                if (target.GetComponent<HealthBar>() != null)
                    target.GetComponent<HealthBar>().TakeDamage(weaponDamage);
            }
        }

        private void OnTriggerEnter(Collider magdetect)
        {
            if (magdetect.gameObject.name.Contains("Magazine") && magazine == null)
            {
                ammoCount = ammoCapacity;

                magdetect.gameObject.transform.parent = gun.transform;
                magdetect.gameObject.transform.localPosition = new Vector3(0, 0, 0);//magwell.transform.position;
                magdetect.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 1);//magwell.transform.rotation;
                magdetect.gameObject.transform.localScale = new Vector3(1, 1, 1);//magwell.transform.localScale;

                magdetect.gameObject.GetComponent<Rigidbody>().useGravity = false;
                magdetect.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                magdetect.gameObject.GetComponent<Collider>().enabled = false;

                magdetect.gameObject.GetComponent<NVRInteractableItem>().AttachedHand = null;
                magdetect.gameObject.name = "CURR_MAG";
            }
        }
    }
}
