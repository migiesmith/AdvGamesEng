using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace space {
    public class PrefabDatabase : MonoBehaviour {

        public GameObject[] prefabs;
        public Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>();
        // Use this for initialization
        void Start() {
            foreach(GameObject prefab in prefabs)
            {
                prefabLookup.Add(prefab.name, prefab);
            }
        }

        public GameObject getPrefab(string name)
        {
            return prefabLookup[name];
        }
    }
}
