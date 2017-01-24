using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShipController : MonoBehaviour {

	private Vector3 initialPos;

	private bool returnToStart = false;

	// Use this for initialization
	void Start () {
		this.initialPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// If this is not being held by the player
		if(returnToStart){
			// Move towards the initial position
			this.transform.position = Vector3.Lerp(this.transform.position, initialPos, 0.1f);

			// If close to initial position, snap to it
			if(Vector3.Magnitude(this.transform.position - initialPos) <= 0.1f){
				this.transform.position = initialPos;
				returnToStart = false;
			}
		}
	}

	public void OnPickUp(){
		returnToStart = false;
	}

	public void OnDrop(){
		returnToStart = true;
	}

}
