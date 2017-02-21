using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Sword : MonoBehaviour
    {
        private NVRInteractableItem sword;
        public Collider blade;

        public float bladeDamage = 25.0f;
        public float bluntDamage = 1.0f;
        public float kineticScaling = 0.5f;

        private void Start()
        {
            sword = this.GetComponent<NVRInteractableItem>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider bladeCollision)
        {
            if (bladeCollision.transform.gameObject.GetComponent<HealthBar>() != null)
            {
                float weaponDamage = kineticScaling * Vector3.Magnitude(this.GetComponent<Rigidbody>().GetRelativePointVelocity(bladeCollision.transform.position));
                if (weaponDamage > bladeDamage)
                    weaponDamage = bladeDamage;

                bladeCollision.transform.gameObject.GetComponent<HealthBar>().TakeDamage(weaponDamage);
            }
            else
                blade.isTrigger = false;
        }

        private void OnCollisionEnter(Collision bluntCollision)
        {
            if (bluntCollision.transform.gameObject.GetComponent<HealthBar>() != null)
                bluntCollision.transform.gameObject.GetComponent<HealthBar>().TakeDamage(bluntDamage);
        }

        private void OnCollisionExit(Collision bluntExit)
        {
            blade.isTrigger = true;
        }
    }
}
