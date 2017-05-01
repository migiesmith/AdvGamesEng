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
        private GameObject dsInst = null;
        private SceneLoadTest loader;
        private Persistence persistence;

        public float respawnDelay = 5.0f;
        private float timer;
        private bool dead;

        public NVRHand[] hands;

        private float prevHealth;
        public ushort hapticStrength = 500;
        private NVRPlayer player;

        private void Start()
        {
            currentHealth = healthPool;
            prevHealth = currentHealth;
            healthBar.updateHealth(healthPool, currentHealth);
            timer = respawnDelay;
            persistence = FindObjectOfType<Persistence>();
            player = transform.root.GetComponent<NVRPlayer>();
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
                            loader.loadScene();
                        else
                        {
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
            else
            {
                if (currentHealth < healthPool)
                {
                    if (timer > 0)
                        timer -= Time.deltaTime;
                    else
                        Heal(5.0f * Time.deltaTime);
                }
                
                if (prevHealth != currentHealth)
                {
                    healthBar.updateHealth(currentHealth);
                    if (prevHealth > currentHealth)
                    {
                        player.LeftHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
                        player.RightHand.TriggerHapticPulse(hapticStrength, NVRButtons.Touchpad);
                    }
                }
                prevHealth = currentHealth;
            }
        }

        public void TakeDamage(float damage)
        {
            //prevHealth = currentHealth;
            if (currentHealth > 0)
            {
                currentHealth -= damage;

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
            //prevHealth = currentHealth;
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
