using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace space
{
    public class HealthBar : MonoBehaviour
    {
        public float healthPool = 100.0f;
        public float currentHealth;
        public float minDamagingVelocity = 5.0f;
        public float collisionDamageScaling = 1.0f;
        private DamageText damageText;
        private bool shielded = false;

        public UnityEvent onDeath;

        private void Start()
        {
            currentHealth = healthPool;
            damageText = GameObject.Find("DamageTextWrapper").GetComponent<DamageText>();
            if (onDeath.GetPersistentEventCount() == 0) {
                onDeath.AddListener(delegate { die(); });
            }
        }

        public void TakeDamage(float damage)
        {
            if (!shielded && currentHealth > 0)
            {
                currentHealth -= damage;
                if (damageText != null)
                    damageText.displayDamage(new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), damage, Color.white);

                if (currentHealth <= 0)
                {
                    onDeath.Invoke();
                    die();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Rigidbody>() != null && Vector3.Magnitude(collision.relativeVelocity) > minDamagingVelocity && !shielded)
                this.TakeDamage(Vector3.Magnitude(collision.relativeVelocity) * collision.gameObject.GetComponent<Rigidbody>().mass * collisionDamageScaling);
        }

        public void die() {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Destroy(this.gameObject, 2.0f);
        }

        public void shieldDown()
        {
            shielded = false;
        }

        public void shieldUp()
        {
            shielded = true;
        }
    }
}