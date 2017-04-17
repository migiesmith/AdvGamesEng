using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class PlayerHealth : MonoBehaviour
    {
        public HealthIndicator healthBar;

        public float healthPool = 100.0f;
        public float currentHealth;

        private void Start()
        {
            currentHealth = healthPool;
            healthBar.updateHealth(healthPool, currentHealth);
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            healthBar.updateHealth(currentHealth);

            if (currentHealth <= 0)
                playerDeath();
        }

        public void Heal(float health)
        {
            currentHealth += health;
            healthBar.updateHealth(currentHealth);
        }

        private void playerDeath()
        {
            Debug.Log("You are dead");
        }
    }
}
