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
        private Light flash;

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
        public Decal laserBurn;
        public ParticleSystem laserParticles;

        public float decalLifetime = 5.0f;

        private void Start()
        {
            line = this.GetComponent<LineRenderer>();
            ammoCount = 0;
            line.material.SetColor("_Color", lineColour);
            NVRHelpers.LineRendererSetColor(line, lineColour, lineColour);
            NVRHelpers.LineRendererSetWidth(line, lineWidth, lineWidth);
            line.enabled = false;
            line.numPositions = 2;

            flash = gun.GetComponentInChildren<Light>();
            flash.enabled = false;
        }

        void Update()
        {
            Transform magazineChild = gun.transform.FindChild("CURR_MAG");
            if(magazineChild != null)
                magazine = magazineChild.gameObject;
            if (magazine != null)
                magazine.gameObject.transform.position = magwell.transform.position;

            if (gun.AttachedHand == null)
            {
                pulseActive = false;
                line.enabled = false;
            }

            if (gun.AttachedHand != null && gun.AttachedHand.UseButtonDown && refireDelay <= 0)
            {
                if (ammoCount > 0)
                {
                    pulseDuration = 0.5f;
                    pulseActive = true;
                    fire();
                    line.enabled = true;
                    flash.enabled = true;
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
                        magazine = null;
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
                flash.enabled = false;
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

                Decal burn = Instantiate(laserBurn, hitInfo.point, Quaternion.FromToRotation(Vector3.back, hitInfo.normal));
                Destroy(burn.gameObject, decalLifetime);
            }
        }

        private void OnTriggerEnter(Collider magdetect)
        {
            if (magdetect.gameObject.name.Contains("Magazine") && magazine == null)
            {
                ammoCount = ammoCapacity;

                magdetect.gameObject.GetComponent<NVRInteractableItem>().ForceDetach();
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
