using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
		ShopValues vals = other.transform.root.GetComponent<ShopValues>();
        if(vals != null)
        	vals.sell();
    }
}
