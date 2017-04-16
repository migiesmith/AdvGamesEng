using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace space {
    public class PrefabDatabase : MonoBehaviour {

        public GameObject[] prefabs;
        public Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>();
        private bool initialised = false;
        // Use this for initialization
        void Start() {
            if (!initialised)
                initialise();
        }

        public GameObject getPrefab(string name)
        {
            if (!initialised)
                initialise();
            return prefabLookup[name];
        }

        private void initialise()
        {
            foreach (GameObject prefab in prefabs)
            {
                prefabLookup.Add(prefab.name, prefab);
            }
            initialised = true;
        }
    }
}
