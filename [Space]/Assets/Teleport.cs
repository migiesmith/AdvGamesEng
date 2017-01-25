using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : MonoBehaviour {

	public int points = 14;

	public float strength = 10.0f;

	public Transform target;

	public Transform toMove;

	private LineRenderer lineRend;


	// Use this for initialization
	void Start () {
		lineRend = this.GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update () {
		// Set the number of points we want on the line
		lineRend.numPositions = points;

		Vector3 targetDir = this.target.position - this.transform.position;

		// Calculate the angle of the arc
		// Z-Axis
		float angle = Mathf.PI/2.0f - Vector3.Dot(Vector3.Normalize(new Vector3(0, targetDir.y, targetDir.z)), new Vector3(0, 0, 1));
		// X-Axis
		float angle2 =  Mathf.PI/2.0f - Vector3.Dot(Vector3.Normalize(new Vector3(targetDir.x, targetDir.y, 0)), new Vector3(1, 0, 0));

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
			Vector3 pos = Vector3.Normalize(new Vector3(targetDir.x, 0, targetDir.z)) * Mathf.Abs(dH) + this.transform.up * dV;			
			// Increment arc progress
			t += (-vV / Physics.gravity.y * 3.0f) / (points - 1);
			// Set the line position
			lineRend.SetPosition(i, this.transform.position + pos);
		}
		

		if(Input.anyKeyDown){
			teleport();
		}

	}

	void teleport(){		
		Debug.Log("Key");
		for(int i = 2; i < lineRend.numPositions; i++){
			Vector3 start = lineRend.GetPosition(i-1);
			Vector3 dir =  lineRend.GetPosition(i) - start;
			RaycastHit rayHit;
			float rayLength = dir.magnitude*2;
			dir = Vector3.Normalize(dir);
				
			Debug.DrawRay(start, dir, Color.blue, rayLength);

			if(Physics.Raycast(start, dir, out rayHit, rayLength)){
				NavMeshHit navHit;
				if(NavMesh.SamplePosition(rayHit.point, out navHit, 0.1f, NavMesh.AllAreas)){
					toMove.position = rayHit.point;
				}
				return;
			}
		}
	}

}
