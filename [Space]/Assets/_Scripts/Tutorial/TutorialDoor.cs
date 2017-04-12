using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used instead of Grant's Door Controller/Slider classes to control when the door is to be opened.
public class TutorialDoor : MonoBehaviour {

    Animation anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        anim.Play();
	}
}
