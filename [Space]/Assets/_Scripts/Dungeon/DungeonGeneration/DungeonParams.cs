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
	[SerializeField] public List<ItemGeneration.ItemWeight> items = new List<ItemGeneration.ItemWeight>();


	// Enemies
	[SerializeField] public List<EnemyGeneration.EnemyWeight> enemies = new List<EnemyGeneration.EnemyWeight>();
	[SerializeField, RangeAttribute(0, 1)] public float enemySpawnRate = 1.0f;


}
