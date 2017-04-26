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
        public float healDelay = 5.0f;

        public GameObject deathScreen;
        public TextMesh deathMessage;
        public SteamVR_LoadLevel loader;

        public float respawnDelay = 5.0f;
        private float timer;
        private bool dead;

        private void Start()
        {
            currentHealth = healthPool;
            healthBar.updateHealth(healthPool, currentHealth);
            deathMessage.text = "";
            deathScreen.SetActive(false);
            timer = respawnDelay;
        }

        void Update()
        {
            if(dead)
            {
                if (timer > 0)
                    timer -= Time.deltaTime;
                else
                {
                    if (FindObjectOfType<Persistence>().tutorialDone)
                    {
                        loader.levelName = "Ship";
                        loader.Trigger();
                    }
                    else
                    {
                        transform.root.position = new Vector3(0.0f, 0.0f, 7.0f);
                    }
                    dead = false;
                    currentHealth = healthPool;
                    deathMessage.text = "";
                    deathScreen.SetActive(false);
                }
            }
            else if(currentHealth < healthPool)
            {
                if (timer > 0)
                    timer -= Time.deltaTime;
                else
                    Heal(5.0f * Time.deltaTime);
            }
        }

        public void TakeDamage(float damage)
        {
            if (currentHealth > 0)
            {
                currentHealth -= damage;
                healthBar.updateHealth(currentHealth);

                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    playerDeath();
                }
                else
                    timer = healDelay;
            }
        }

        public void Heal(float health)
        {
            currentHealth += health;
            if (currentHealth > healthPool)
                currentHealth = healthPool;
            healthBar.updateHealth(currentHealth);
        }

        private void playerDeath()
        {
            deathScreen.SetActive(true);
            deathMessage.text = "You are dead";
            dead = true;
            timer = respawnDelay;
            FindObjectOfType<LootInventory>().clearLoot();
        }
    }
}
