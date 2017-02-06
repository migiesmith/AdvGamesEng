using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : Pathfinding {

	public Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null){
			//If i hit the P key i will get a path from my position to my end position
			if (Input.GetKeyDown(KeyCode.P))
			{	
				FindPath(transform.position, target.position);
				Debug.Log(Path.Count);
			}
			//If path count is bigger than zero then call a move method
			if (Path.Count > 0)
			{
				Move();
			}
		}
	}
}
