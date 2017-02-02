using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace space
{
    public class Sword : MonoBehaviour
    {
        public Collider blade;
        public Collider tip;

        public float weaponDamage = 20.0f;



        // Update is called once per frame
        void Update()
        {
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.gameObject.GetComponent<HealthBar>() != null)
                collision.transform.gameObject.GetComponent<HealthBar>().TakeDamage(weaponDamage);
        }
    }
}
