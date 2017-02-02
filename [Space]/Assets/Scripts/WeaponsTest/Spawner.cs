using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace space
{

    public class Spawner : MonoBehaviour
    {
        public float respawnDelay = 5.0f;
        private float respawnTimer;
        public GameObject targetModel;
        private GameObject currentModel;

        // Use this for initialization
        void Start()
        {
            currentModel = Instantiate(targetModel, transform.position, transform.rotation);
            currentModel.transform.parent = transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.childCount == 0)
            {
                if (respawnTimer <= 0)
                {
                    currentModel = Instantiate(targetModel, transform.position, transform.rotation);
                    currentModel.transform.parent = transform;
                    respawnTimer = respawnDelay;
                }
                else
                    respawnTimer -= Time.deltaTime;
            }
        }
    }
}
                

