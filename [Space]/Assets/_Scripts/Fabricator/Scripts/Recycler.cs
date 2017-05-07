using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour {

    private space.ItemSpawn spawner;
	// Use this for initialization
	void Start () {
        spawner = FindObjectOfType<space.ItemSpawn>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        NewtonVR.NVRInteractable otherInt = other.transform.root.GetComponentInChildren<NewtonVR.NVRInteractable>();
        if (otherInt != null && !otherInt.IsAttached)
        {
            ShopValues vals = other.transform.root.GetComponent<ShopValues>();
            if (vals != null)
            {
                otherInt.enabled = false;
                vals.organics = Mathf.RoundToInt(0.8f*vals.organics);
                vals.metals = Mathf.RoundToInt(0.8f * vals.metals);
                vals.fuel = Mathf.RoundToInt(0.8f * vals.fuel);
                vals.radioactive = Mathf.RoundToInt(0.8f * vals.radioactive);
                vals.sell();
                spawner.dissolveOut(other.gameObject);
                spawner.updateResources();
            }
        }
    }
}
