/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonParams {

	[System.Serializable]
	public struct ItemWeight
	{
		[SerializeField] public Item item;
		[SerializeField] public float weight;
	}

    // The nubmer of rooms that will be generated as a set for connecting (not necesarily the room count)
	[SerializeField] public int RoomPoolSize = 100;
	[SerializeField, RangeAttribute(0, 1)] public float enemySpawnRate = 1.0f;
	[SerializeField] public List<ItemWeight> items = new List<ItemWeight>();
}
