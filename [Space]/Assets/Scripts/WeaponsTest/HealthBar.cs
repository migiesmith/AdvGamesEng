using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace space
{
    public class HealthBar : MonoBehaviour
    {
        public float healthPool = 100.0f;
        private float currentHealth;
        public float minDamagingVelocity = 5.0f;
        public float collisionDamageScaling = 1.0f;
        private DamageText damageText;

        private void Start()
        {
            currentHealth = healthPool;
            damageText = GameObject.Find("DamageTextWrapper").GetComponent<DamageText>();
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (damageText != null)
                damageText.displayDamage(new Vector3(transform.position.x, 2.0f, transform.position.z), damage);
            
            if (currentHealth <= 0)
            {
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Destroy(this.gameObject, 2.0f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Rigidbody>() != null && Vector3.Magnitude(collision.relativeVelocity) > minDamagingVelocity)
                this.TakeDamage(Vector3.Magnitude(collision.relativeVelocity) * collision.gameObject.GetComponent<Rigidbody>().mass * collisionDamageScaling);
        }
    }
}