using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ParticleCollisionRelay : MonoBehaviour
    {
        private float actualDPS;
        // Use this for initialization
        void Start()
        {
            if(transform.root.GetComponent<Flamethrower>() != null)
                actualDPS = transform.root.GetComponent<Flamethrower>().actualDPS;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnParticleCollision(GameObject target)
        {
            HealthBar targetHealth = target.GetComponent<HealthBar>();
            ShieldBar targetShield = target.GetComponent<ShieldBar>();
            if (targetShield != null)
            {
                if (!targetShield.down)
                    targetShield.TakeDamage(actualDPS * Time.deltaTime, Vector3.zero);
                else if (targetHealth != null)
                    targetHealth.TakeDamage(actualDPS * Time.deltaTime);
            }
            else if (targetHealth != null)
                targetHealth.TakeDamage(actualDPS * Time.deltaTime);
        }
    }
}
