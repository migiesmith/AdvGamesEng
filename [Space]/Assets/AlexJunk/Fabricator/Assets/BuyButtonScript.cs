using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            Shop2.spawn=true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Shop2.spawn = true;
    }
}
