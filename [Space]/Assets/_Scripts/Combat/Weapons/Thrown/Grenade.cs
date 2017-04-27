using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractableItem))]
    public class Grenade : MonoBehaviour
    {
        private NVRInteractableItem grenade;
        public float fuse = 5.0f;
        private bool triggered = false;
        public float blastRadius = 10.0f;
        public float blastForce = 100.0f;
        public float weaponDamage = 100.0f;
        public float speedBoost = 3.0f;
        private Light flash;
        private ParticleSystem explosion;

        private void Start()
        {
            grenade = this.GetComponent<NVRInteractableItem>();
            flash = this.GetComponent<Light>();
            flash.enabled = false;
            explosion = this.GetComponent<ParticleSystem>();
        }
        
        void Update()
        {
            if (triggered == true)
            {
                if (fuse > 0)
                    fuse -= Time.deltaTime;
                else
                {
                    detonate();
                    triggered = false;
                }
            }
            else if (grenade.AttachedHand != null && grenade.AttachedHand.UseButtonDown)
                triggered = true;
        }

        void detonate()
        {
            Collider[] blastZone = Physics.OverlapSphere(grenade.transform.position, blastRadius);
            
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Collider>().enabled = false;

            foreach (Collider hit in blastZone)
            {
                Rigidbody targetRB = hit.transform.gameObject.GetComponent<Rigidbody>();
                HealthBar targetHealth = hit.transform.gameObject.GetComponent<HealthBar>();
                ShieldBar targetShield = hit.transform.gameObject.GetComponent<ShieldBar>();

                if (targetRB != null)
                    targetRB.AddExplosionForce(blastForce, grenade.transform.position, blastRadius);

                if (targetShield != null)
                {
                    if (!targetShield.down)
                        targetShield.TakeDamage(weaponDamage, hit.ClosestPointOnBounds(transform.position));
                    else if (targetHealth != null)
                        targetHealth.TakeDamage(weaponDamage);
                }
                else if (targetHealth != null)
                    targetHealth.TakeDamage(weaponDamage);
            }
            
            flash.enabled = true;
            explosion.Play();
            this.GetComponent<MeshRenderer>().enabled = false;
            Destroy(this.gameObject, 0.5f);
        }
    }
}
