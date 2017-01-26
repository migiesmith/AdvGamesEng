using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammedDoorController : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        this.animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("proximitySensor", true);
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("proximitySensor", false);
    }
}
