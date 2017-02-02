using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractableItem))]
    public class Grenade : MonoBehaviour
    {
        public NVRInteractableItem grenade;
        public float fuse = 5.0f;
        private bool triggered = false;
        public float blastRadius = 10.0f;
        public float blastForce = 100.0f;
        public float weaponDamage = 100.0f;

        private void Start()
        {
            this.triggered = false;
        }

        void Update()
        {
            if (grenade.AttachedHand != null && grenade.AttachedHand.UseButtonDown && this.triggered == false)
            {
                this.triggered = true;
                tick();
            }
            else if (this.triggered == true)
            {
                if (fuse > 0)
                    tick();
                else
                    detonate();
            }
        }

        void tick()
        {
            this.fuse -= Time.deltaTime;
        }

        void detonate()
        {
            Collider[] blastZone = Physics.OverlapSphere(grenade.transform.position, blastRadius);

            foreach (Collider hit in blastZone)
            {
                GameObject target = hit.transform.gameObject;

                if (target.GetComponent<Rigidbody>() != null)
                    target.GetComponent<Rigidbody>().AddExplosionForce(blastForce, grenade.transform.position, blastRadius);

                if (target.GetComponent<HealthBar>() != null)
                    target.GetComponent<HealthBar>().TakeDamage(weaponDamage);
            }
            Destroy(this.gameObject);
        }
    }
}
