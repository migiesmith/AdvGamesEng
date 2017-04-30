﻿using System.Collections;
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
        private GameObject dsInst = null;
        private SceneLoadTest loader;
        private Persistence persistence;

        public float respawnDelay = 5.0f;
        private float timer;
        private bool dead;

        private void Start()
        {
            currentHealth = healthPool;
            healthBar.updateHealth(healthPool, currentHealth);
            timer = respawnDelay;
            persistence = FindObjectOfType<Persistence>();
        }

        void Update()
        {
            if (dead)
            {
                if (timer > 0)
                    timer -= Time.deltaTime;
                else
                {
                    if (persistence != null)
                    {
                        if (FindObjectOfType<Persistence>().tutorialDone)
                        {
                            Debug.Log("Tutorial complete.");
                            loader.loadScene();
                        }
                        else
                        {
                            Debug.Log("Tutorial not complete.");
                            transform.root.position = new Vector3(0.0f, 0.0f, 7.0f);
                            if (dsInst != null)
                                Destroy(dsInst);
                        }
                    }
                    dead = false;
                    currentHealth = healthPool;
                    healthBar.updateHealth(currentHealth);

                }
            }
            else if (currentHealth < healthPool)
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
            dead = true;
            timer = respawnDelay;
            FindObjectOfType<LootInventory>().clearLoot();

            if (dsInst == null)
            {
                dsInst = Instantiate(deathScreen, transform.parent);
                loader = dsInst.GetComponent<SceneLoadTest>();
                dsInst.transform.localPosition = new Vector3(0.0f, 0.0f, 0.3f);
            }
        }
    }

}
