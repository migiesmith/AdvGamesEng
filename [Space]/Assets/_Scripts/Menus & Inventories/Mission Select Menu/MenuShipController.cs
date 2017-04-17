using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SteamVR_LoadLevel))]
public class MenuShipController : MonoBehaviour {

	public float snapDist = 0.01f;

	public GameObject uiObject = null;

	public Material lineMat = null;

	private Vector3 initialPos;
	private Quaternion initialRotation;

	private LineRenderer lineRend;

	private bool returnToStart = false;

	private SteamVR_LoadLevel levelLoader;

    private NewtonVR.NVRPlayer player;

	// Use this for initialization
	void Start () {
		this.initialPos = this.transform.position;
		this.initialRotation = this.transform.rotation;

		this.lineRend = this.GetComponent<LineRenderer>();
		if(this.lineRend != null){
			this.lineRend.material = lineMat;
		}

		this.levelLoader = this.GetComponent<SteamVR_LoadLevel>();
        player = FindObjectOfType<NewtonVR.NVRPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		// If this is not being held by the player
		if(this.returnToStart){

			Rigidbody rb = this.GetComponent<Rigidbody>();
			if(rb != null){
				rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
				rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
			}


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

	public void startLevel(){
		levelLoader.enabled = true;
        DontDestroyOnLoad(player.gameObject);
        if (player.LeftHand.CurrentlyInteracting != null && player.LeftHand.CurrentlyInteracting.transform.root.gameObject != transform.root.gameObject)
            DontDestroyOnLoad(player.LeftHand.CurrentlyInteracting.transform.root.gameObject);
        if (player.RightHand.CurrentlyInteracting != null && player.LeftHand.CurrentlyInteracting.transform.root.gameObject != transform.root.gameObject)
            DontDestroyOnLoad(player.RightHand.CurrentlyInteracting.transform.root.gameObject);
        levelLoader.Trigger();
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
