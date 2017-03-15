using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace space
{
    public class BulletDamage : MonoBehaviour
    {

        public float weaponDamage = 10;

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.transform.gameObject.GetComponent<HealthBar>() != null)
            {
                collision.transform.gameObject.GetComponent<HealthBar>().TakeDamage(weaponDamage);
                Destroy(this);
            }

            if (collision.transform.gameObject.GetComponent<PlayerHealth>() != null)
            {
                collision.transform.gameObject.GetComponent<PlayerHealth>().TakeDamage(weaponDamage);
                Destroy(this);
            }
        }
    }
}
 

