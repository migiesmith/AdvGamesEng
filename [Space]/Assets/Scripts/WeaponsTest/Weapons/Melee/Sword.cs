using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Sword : MonoBehaviour
    {
        private Rigidbody swordRB;
        public float bladeDamageScaling = 1.0f;
        private bool damageLive;

        private void Start()
        {
            swordRB = GetComponent<Rigidbody>();
            damageLive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<HealthBar>() != null)
                damageLive = true;
        }

        private void OnTriggerExit(Collider other)
        {
            damageLive = false;
        } 

        private void OnCollisionEnter(Collision collision)
        {
            HealthBar enemyHealth = collision.gameObject.GetComponent<HealthBar>();
            if (enemyHealth != null && damageLive)
                enemyHealth.TakeDamage(bladeDamageScaling * swordRB.mass * Vector3.Magnitude(collision.relativeVelocity));
            damageLive = false;
        }
    }
}
