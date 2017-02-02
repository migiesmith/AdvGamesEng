using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace space
{
    public class HealthBar : MonoBehaviour
    {
        public float healthPool = 100.0f;
        private float currentHealth;

        private void Start()
        {
            currentHealth = healthPool;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
                Destroy(this.gameObject);
        }
    }
}