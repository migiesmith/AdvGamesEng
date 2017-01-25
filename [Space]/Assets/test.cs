using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	public int points = 9;

	public float strength = 10.0f;

	public Transform target;

	private LineRenderer lineRend;


	// Use this for initialization
	void Start () {
		lineRend = this.GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update () {
		// Set the number of points we want on the line
		lineRend.numPositions = points;

		// Calculate the angle of the arc
		// Z-Axis
		float angle = Mathf.PI/2.0f - Vector3.Dot(Vector3.Normalize(new Vector3(0, target.position.y, target.position.z)), new Vector3(0, 0, 1));
		// X-Axis
		float angle2 =  Mathf.PI/2.0f - Vector3.Dot(Vector3.Normalize(new Vector3(target.position.x, target.position.y, 0)), new Vector3(1, 0, 0));

		// Set angle to angle2 if we are closer to the X-Axis
		if(Mathf.Abs(target.transform.position.x) > Mathf.Abs(target.transform.position.z)){
			angle = angle2;
		}

		// Calculate the velocity
		float vV = strength * Mathf.Sin(angle); // Vertical - Y
		float vH = strength * Mathf.Cos(angle); // Horizontal - X and Z

		// Store the progress through the arc
		float t = 0.0f;

        for (int i = 0 ; i < points; i++) {
			// Horizontal Displacement - X and Z
			float dH = vH * t;
			// Vertical Displacement - Y
			float dV = (vV * t) + 0.5f * Physics.gravity.y * t * t;
			// Create the position for the line
			Vector3 pos = Vector3.Normalize(new Vector3(target.position.x, 0, target.position.z)) * Mathf.Abs(dH) + this.transform.up * dV;			
			// Increment arc progress
			t += (-vV / Physics.gravity.y * 2.0f) / (points - 1);
			// Set the line position
			lineRend.SetPosition(i, pos);
		}
		

	}
}
