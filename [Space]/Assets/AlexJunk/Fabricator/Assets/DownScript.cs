using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("w"))
        {
            Shop2.curr++;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Shop2.curr++;
    }
}
