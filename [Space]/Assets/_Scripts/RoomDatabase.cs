/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDatabase : MonoBehaviour {

	public List<GameObject> rooms = new List<GameObject>();

	protected Dictionary<string, GameObject> roomsMap = new Dictionary<string, GameObject>();

	void Awake()
	{
		for(int i = 0; i < rooms.Count; ++i)
		{
			roomsMap.Add(rooms[i].name, rooms[i]);
		}
	}

	public GameObject getRoom(string name)
	{
		return roomsMap[name];
	}

}