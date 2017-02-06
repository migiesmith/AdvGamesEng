using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("Open", true);

        // If the collider entered while closing
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Closing")){
            animator.CrossFade("Open", animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 0.5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        animator.SetBool("Open", true);        
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("Open", false);

        // If the collider left while opening
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Opening")){
            animator.CrossFade("Closed", 1.0f - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
    }
}
