using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public Animator animator;

    bool isOpen = false;

	// Use this for initialization
	void Start () {
        // TODO this is a crude solution to stop door animations from resetting
        this.transform.parent.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if(isOpen)
            return;

        if(other.tag == "PlayerSensor" || other.tag == "EnemySensor"){
            animator.SetBool("Open", isOpen = true);
            Debug.Log(other +", "+ other.tag +","+ (other.tag == "PlayerSensor"));

            // If the collider entered while closing
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Closing")){
                animator.CrossFade("Open", animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 0.5f);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(isOpen)
            return;

        if(other.tag == "PlayerSensor" || other.tag == "EnemySensor"){            
            animator.SetBool("Open", true);   
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if(!isOpen)
            return;

        if(other.tag == "PlayerSensor" || other.tag == "EnemySensor"){
            animator.SetBool("Open", isOpen = false);

            // If the collider left while opening
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Opening")){
                animator.CrossFade("Closed", 1.0f - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
    }
}
