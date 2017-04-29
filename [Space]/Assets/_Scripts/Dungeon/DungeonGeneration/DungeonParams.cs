/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonParams {

	// Core generation params

    // The nubmer of rooms that will be generated as a set for connecting (not necesarily the room count)
	[SerializeField] public int RoomPoolSize = 100;

	// The objective object
	[SerializeField] public GameObject objective;



	// Items

	[System.Serializable]
	public class ItemWeight
	{
		[SerializeField] public Item item;
		[SerializeField] public float weight;
	}
	[SerializeField] public List<ItemWeight> items = new List<ItemWeight>();


	// Enemies
	
	[System.Serializable]
	public class EnemyWeight
	{
		[SerializeField] public Enemy enemy;
		[SerializeField] public float weight;
	}
	[SerializeField] public List<EnemyWeight> enemies = new List<EnemyWeight>();
	[SerializeField, RangeAttribute(0, 1)] public float enemySpawnRate = 1.0f;


}
