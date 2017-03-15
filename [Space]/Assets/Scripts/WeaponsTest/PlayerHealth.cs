using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class PlayerHealth : MonoBehaviour
    {
        public float healthPool = 100.0f;
        public float currentHealth;

        private void Start()
        {
            currentHealth = healthPool;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
                playerDeath();
        }

        private void playerDeath()
        {
            Debug.Log("You are dead");
        }
    }
}
