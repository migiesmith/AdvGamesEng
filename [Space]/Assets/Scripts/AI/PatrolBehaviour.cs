﻿/*
 enemy will patrol an area
 */ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolBehaviour : Behaviour {

	//enemy that the behaviour will control
	private Enemy enemy;

	List<Vector3> patrol_route;

	//use to determine which point enemy should head towards
	private int patrol_index = 0;

	//number of points
	private int route_size;

	float speed = 15.0f;

	private bool isReady = false;

	//empty constructor
	public PatrolBehaviour(){

	}

	//set enemy
	public PatrolBehaviour(Enemy e){
		patrol_route = new List<Vector3>();


		this.enemy = e;

		/*
		patrol_route.Add (new Vector2 (-2, 0));
		patrol_route.Add (new Vector2 (2, 0));
		patrol_route.Add (new Vector2 (2, 2));
		patrol_route.Add (new Vector2 (-2, 2));
		*/

        enemy.StartCoroutine(LateStart(2));
     }
 
     IEnumerator LateStart(float waitTime)
     {
        yield return new WaitForSeconds(waitTime);

		WaypointNode[] nodes = GameObject.FindObjectsOfType<WaypointNode>();
		for(int i = 0; i < 6; i++)
			patrol_route.Add(nodes[Random.Range(0, nodes.Length)].transform.position);

		enemy.transform.position = patrol_route[0];

		isReady = true;
    }



	//update
	public void update(){

		if(!isReady)
			return;

		//get destination
		Vector3 current_destination = patrol_route[patrol_index];

		float distance = Vector3.Distance(enemy.transform.position, current_destination);
		
		//change destination if enemy has reached previous one
		if(distance < 0.4f){
			patrol_index++;

			//loop patrol index
			if (patrol_index >= patrol_route.Count)
				patrol_index = 0;

				enemy.FindPath(enemy.transform.position, patrol_route[patrol_index]);
		}
		// TODO remove debug rendering
		Debug.DrawLine(enemy.transform.position, patrol_route[patrol_index]);

		if (enemy.Path.Count > 0){
			enemy.move(speed);
		}else{
			Debug.Log("No Path.");
		}
		
	}

}
