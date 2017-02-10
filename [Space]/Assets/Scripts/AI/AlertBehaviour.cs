/*
 When enemy has been alerted to the players presence, it will attempt to find the player
 */


using UnityEngine;
using System.Collections;

public class AlertBehaviour : Behaviour {

	private Enemy enemy;

	float rotationleft=0;
	float rotationspeed=100;

	//empty constructor
	public AlertBehaviour(){
		
	}

	public AlertBehaviour(Enemy e){
		this.enemy = e;
	}

	public void SetRotation(float angle){
		this.rotationleft = angle;
	}

	public void update(){

		//rotate 360 degrees to search for player
		float rotation=rotationspeed*Time.deltaTime;
		if (rotationleft > rotation){
			rotationleft-=rotation;
			RaycastHit hit;
			Debug.DrawRay (enemy.transform.position, enemy.transform.forward * 50, Color.red);
			if (Physics.Raycast (enemy.transform.position, enemy.transform.forward, out hit)) {
				//Debug.Log ("************");
				//enemy.ToCombat();
				if (hit.collider.tag.Equals("Player")) {
					enemy.ToCombat ();
				Debug.Log (hit.collider.name);
				}
			}
		}
		else{
			rotation=rotationleft;
			rotationleft=0;
			enemy.ToPatrol ();
		}

		enemy.transform.Rotate(0,rotation,0);
	}
}
