/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneration {

	private RoomBehaviour roomBehav;

	public ItemGeneration(RoomBehaviour rb)
	{
		this.roomBehav = rb;
	}

	public DungeonParams getParams()
	{
		return roomBehav.getParams();
	}

	public void generate()
	{
        List<Transform> lootAreas = roomBehav.transform.FindDeepChildren("LootArea");
		foreach(Transform area in lootAreas)
		{
			Item toSpawn =  pickItem();
			if(toSpawn != null)
			{
				GameObject go = null;
				if(toSpawn.prefab != null)
				{
					go = GameObject.Instantiate(toSpawn.prefab);
				}
				else
				{					
					go = GameObject.CreatePrimitive(PrimitiveType.Cube);
				}
				go.transform.position = area.position;
				go.name = toSpawn.name;
				go.transform.parent = roomBehav.transform;	
			}
		}
	}

	Item pickItem()
	{
		DungeonParams param = getParams();
		if(param != null)
		{
			float totalWeight = 0.0f;
			foreach(DungeonParams.ItemWeight iWeight in param.items)
			{
				totalWeight += iWeight.weight;
			}
			
			totalWeight *= Random.Range(0.0f, 1.0f);
			foreach(DungeonParams.ItemWeight iWeight in param.items)
			{
				totalWeight -= iWeight.weight;
				if(totalWeight <= 0.0f)
					return iWeight.item;
			}
			if(param.items.Count > 0)
			{
				return param.items[Random.Range(0, param.items.Count)].item;
			}
		}
		return null;
	}

}
