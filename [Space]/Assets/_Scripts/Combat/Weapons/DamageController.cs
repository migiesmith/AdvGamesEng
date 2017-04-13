using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class DamageController : MonoBehaviour
    {
        private float weaponDamage;

        public void hitTarget(GameObject target, Vector3 position)
        {
            HealthBar targetHealth = target.transform.gameObject.GetComponent<HealthBar>();
            ShieldBar targetShield = target.transform.gameObject.GetComponent<ShieldBar>();

            if (targetShield != null)
            {
                if (!targetShield.down)
                    targetShield.TakeDamage(weaponDamage, position);
                else if (targetHealth != null)
                    targetHealth.TakeDamage(weaponDamage);
            }
            else if (targetHealth != null)
                targetHealth.TakeDamage(weaponDamage);
        }

        public void setParams(float weaponDamageIn)
        {
            weaponDamage = weaponDamageIn;
        }
    }
}
