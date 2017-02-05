using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NewtonVR;

[RequireComponent (typeof (LineRenderer))]
public class Teleport : MonoBehaviour {

	public int points = 14;

	public GameObject projectilePrefab;

	public float strength = 1200.0f;

	public NewtonVR.NVRHand hand;

	public Transform toMove;

	private LineRenderer lineRend;

	private GameObject projectile = null;

    private bool latch;

	// Use this for initialization
	void Start () {
		lineRend = this.GetComponent<LineRenderer>();
		// Setup the line renderer
		lineRend.enabled = false;
		lineRend.numCapVertices = 4;
		lineRend.numPositions = 2;
		NVRHelpers.LineRendererSetWidth(lineRend, 0.02f, 0.01f);

        latch = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (!this.hand.Inputs[NVRButtons.Touchpad].IsPressed && latch == true){
            latch = false;
        }

        if (this.hand.Inputs[NVRButtons.Touchpad].IsPressed && this.hand.Inputs[NVRButtons.Touchpad].Axis.y < -0.1f && latch == false){
            latch = true;
            teleport();
        }
        else if (this.hand.Inputs[NewtonVR.NVRButtons.Touchpad].IsTouched && this.hand.Inputs[NVRButtons.Touchpad].Axis.y < -0.1f){
            drawTeleportDirection();
        }
        else{
            lineRend.enabled = false;
        }
    }

	protected void drawTeleportDirection(){
		// Enable the line renderer
		lineRend.enabled = true;
		lineRend.SetPositions(new Vector3[]{this.hand.transform.position, this.hand.transform.position + this.hand.CurrentForward * 0.1f});
	}

	protected void teleport(){
		// Disable the line renderer
		lineRend.enabled = false;
		Debug.Log("teleport");
		// If we haven't fired a projectile, fire one
		if(projectile == null){
			projectile = (GameObject) Instantiate(projectilePrefab);
			projectile.transform.position = hand.transform.position;
			projectile.GetComponent<Rigidbody>().AddForce(hand.transform.forward * strength);

			TeleportCollide bulletScript = projectile.GetComponent<TeleportCollide>();
			bulletScript.toTeleport = toMove;			
		}
	}
}
