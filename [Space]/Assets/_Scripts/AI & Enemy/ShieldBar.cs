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
        private HealthBar health;
        private ShieldController controller;
        private DamageText damageText;
        public Collider shieldCollider;

        public float rechargeRate = 1.0f;
        public float rechargeDelay = 0.5f;
        public float downDelay = 3.0f;
        private float timer;

        public bool down;
        // Use this for initialization
        void Start()
        {
            shieldHealth = maxShield;
            health = GetComponentInParent<HealthBar>();
            controller = GetComponent<ShieldController>();
            damageText = GameObject.Find("DamageTextWrapper").GetComponent<DamageText>();
            timer = 0.0f;
        }

        void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;

            if (down && timer <= 0)
            {
                health.shieldUp();
                down = false;
            }
            if (shieldHealth < maxShield && timer <= 0 & !down)
                shieldHealth += rechargeRate * Time.deltaTime;
        }

        public void TakeDamage(float damage, Vector3 position)
        {
            if (!down && shieldCollider.enabled)
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
        }
    }
}
