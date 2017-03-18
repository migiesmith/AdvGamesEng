/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// Note: This is only for use as DEBUG input if a vive 
// 		  is not available.
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

[RequireComponent (typeof (NVRHand)), RequireComponent(typeof (SphereCollider))]
public class GripForEditor : MonoBehaviour {

	public KeyCode toggleKey;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(toggleKey)){
			NVRHand hand = this.GetComponent<NVRHand>(); 
			if(hand.CurrentlyInteracting == null){
				hand.HoldButtonDown = true;
				hand.CurrentHandState = HandState.Idle;
				//hand.CurrentInteractionStyle = NewtonVR.InterationStyle.Hold;
				hand.SendMessage("UpdateInteractions");
			}else{
				hand.HoldButtonDown = true;
				//hand.CurrentInteractionStyle = NewtonVR.InterationStyle.Hold;
				hand.SendMessage("UpdateInteractions");
			}
		}
	}
}
