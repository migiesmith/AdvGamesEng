﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class Sword : MonoBehaviour
    {
        private Rigidbody swordRB;
        public float bladeDamageScaling = 1.0f;
        private bool damageLive;

        private void Start()
        {
            swordRB = GetComponent<Rigidbody>();
            damageLive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<HealthBar>() != null)
                damageLive = true;
        }

        private void OnTriggerExit(Collider other)
        {
            damageLive = false;
        } 

        private void OnCollisionEnter(Collision collision)
        {
            Rigidbody targetRB = collision.gameObject.GetComponent<Rigidbody>();
            HealthBar targetHealth = collision.gameObject.GetComponent<HealthBar>();
            ShieldBar targetShield = collision.gameObject.GetComponent<ShieldBar>();

            if (targetShield != null && damageLive)
            {
                if (!targetShield.down)
                    targetShield.TakeDamage(bladeDamageScaling * swordRB.mass * Vector3.Magnitude(collision.relativeVelocity), collision.contacts[0].point);
                else if (targetHealth != null && damageLive)
                    targetHealth.TakeDamage(bladeDamageScaling * swordRB.mass * Vector3.Magnitude(collision.relativeVelocity));
            }
            else if (targetHealth != null)
                targetHealth.TakeDamage(bladeDamageScaling * swordRB.mass * Vector3.Magnitude(collision.relativeVelocity));

            damageLive = false;
        }
    }
}
