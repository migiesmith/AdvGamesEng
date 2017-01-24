using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShipController : MonoBehaviour {

	public float snapDist = 0.01f;

	public GameObject uiObject = null;

	public Material lineMat = null;

	private Vector3 initialPos;
	private Quaternion initialRotation;

	private LineRenderer lineRend;

	private bool returnToStart = false;


	// Use this for initialization
	void Start () {
		this.initialPos = this.transform.position;
		this.initialRotation = this.transform.rotation;

		this.lineRend = this.GetComponent<LineRenderer>();
		if(this.lineRend != null){
			this.lineRend.material = lineMat;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// If this is not being held by the player
		if(this.returnToStart){

			bool locationSnap = false;
			bool rotationSnap = false;

			// Location

			// Move towards the initial position
			this.transform.position = Vector3.Lerp(this.transform.position, this.initialPos, 0.1f);

			// If close to initial position, snap to it
			if(Vector3.Magnitude(this.transform.position - this.initialPos) <= this.snapDist){
				this.transform.position = this.initialPos;
				locationSnap = true;
			}

			// Rotation

			// Rotate towards the initial rotation
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.initialRotation, 0.1f);

			// If close to initial rotation, snap to it
			if(Quaternion.Angle(this.transform.rotation, this.initialRotation) < 0.1f){
				this.transform.rotation = this.initialRotation;
				rotationSnap = false;				
			}

			// If both location and rotation snapped to then we can stop returning
			if(locationSnap && rotationSnap){
				this.returnToStart = false;
			}


		}

		// If we have a line renderer and there is a seperation from initialPos then render a line
		if(this.lineRend != null){
			if(Vector3.Magnitude(this.initialPos - this.transform.position) <= this.snapDist){
				this.lineRend.numPositions = 0;
			}else{
				this.lineRend.numPositions = 2;
				this.lineRend.SetPositions(new Vector3[]{this.initialPos, this.transform.position});
			}
		}

	}

	public void OnPickUp(){
		this.returnToStart = false;

		if(this.uiObject != null){
			this.uiObject.SetActive(true);
		}

	}

	public void OnDrop(){
		this.returnToStart = true;
		if(this.uiObject != null){
			this.uiObject.SetActive(false);
		}
	}

}
