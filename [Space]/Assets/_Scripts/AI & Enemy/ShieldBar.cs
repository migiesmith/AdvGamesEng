using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ShieldBar : MonoBehaviour
    {
        public float maxShield = 100.0f;
        private float shieldHealth;
        public float minDamagingVelocity = 5.0f;
        public float collisionDamageScaling = 1.0f;

        public HealthBar health;
        public ShieldController controller;
        private DamageText damageText;
        public Collider shieldCollider;

        public float rechargeRate = 1.0f;
        public float rechargeDelay = 0.5f;
        public float downDelay = 5.0f;
        private float timer;

        public bool down;
        public bool online;
        // Use this for initialization
        void Start()
        {
            Physics.IgnoreCollision(shieldCollider, GetComponentInParent<CapsuleCollider>());
            shieldHealth = maxShield;
            damageText = GameObject.Find("DamageTextWrapper").GetComponent<DamageText>();
            timer = 0.0f;
        }

        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;

            if (down && timer <= 0)
            {
                shieldCollider.enabled = true;
                controller.reviveShield();
                health.shieldUp();
                down = false;
            }
            if (shieldHealth < maxShield && timer <= 0 & !down)
            {
                shieldHealth += rechargeRate * Time.deltaTime;
                if (shieldHealth > maxShield)
                    shieldHealth = maxShield;
            }
        }

        public void TakeDamage(float damage, Vector3 position)
        {
            if (online && !down && shieldCollider.enabled)
            {
                controller.addHit(position);
                shieldHealth -= damage;
                if (damageText != null)
                    damageText.displayDamage(new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), damage, Color.cyan);

                if (shieldHealth <= 0)
                {
                    shieldCollider.enabled = false;
                    controller.breakShield();
                    health.shieldDown();
                    down = true;
                    timer = downDelay;
                }
                else
                    timer = rechargeDelay;
            }
            else
                health.TakeDamage(damage);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!down && collision.gameObject.GetComponent<Rigidbody>() != null && Vector3.Magnitude(collision.relativeVelocity) > minDamagingVelocity)
                TakeDamage(Vector3.Magnitude(collision.relativeVelocity) * collision.gameObject.GetComponent<Rigidbody>().mass * collisionDamageScaling, collision.contacts[0].point);
        }

        public void disableShield()
        {
            shieldCollider.enabled = false;
            health.shieldDown();
            controller.breakShield();
            online = false;
        }

        public void enableShield()
        {
            shieldCollider.enabled = true;
            health.shieldUp();
            controller.reviveShield();
            online = true;
        }
    }
}
