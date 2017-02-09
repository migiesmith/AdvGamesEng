﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unique layout used for a Basic room.
public class BasicLayout : RoomLayout
{
	public BasicLayout(Room r) : base(r)
	{
        //generateLights();
        generateRoomObjects();
	}

    public override void generateLights()
    {
        GameObject lightGameObject = new GameObject("The Light");
        Light lightComp = lightGameObject.AddComponent<Light>();
        lightComp.color = Color.white;
        lightGameObject.transform.position = new Vector3(room.position.x + room.type.dimensions.x, room.type.dimensions.y / 2, room.position.z - (room.type.dimensions.z / 2)); //Top-left corner light.

        roomLights.Add(lightGameObject);

        GameObject lightGameObject2 = new GameObject("The Light 2");
        Light lightComp2 = lightGameObject2.AddComponent<Light>();
        lightComp2.color = Color.white;
        lightGameObject2.transform.position = new Vector3(room.position.x - room.type.dimensions.x, room.type.dimensions.y / 2, room.position.z + (room.type.dimensions.z / 2)); //bottom-right corner light.

        roomLights.Add(lightGameObject2);
    }

    public override void generateRoomObjects()
    {
        //Chest
        GameObject chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
        chest.transform.position = new Vector3(room.position.x + (room.type.dimensions.x / 4), 0.0f, room.position.z - (room.type.dimensions.z / 4)); //Spawn in top-left corner.
        chest.GetComponent<Renderer>().material.color = Color.blue;

        chest.transform.parent = room.getRoomBehaviour().transform.parent;
        chest.SetActive(false);
        roomObjects.Add(chest);

        //Tables + Chairs
        GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cube);
        table.transform.position = new Vector3(room.position.x + (room.type.dimensions.x / 4), 0.0f, room.position.z + (room.type.dimensions.z / 4)); //Spawn in top-right corner.
        table.GetComponent<Renderer>().material.color = Color.red;

        table.transform.parent = room.getRoomBehaviour().transform.parent;
        table.SetActive(false);
        roomObjects.Add(table);
        
        //Enemy
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        enemy.transform.position = new Vector3(room.position.x - (room.type.dimensions.x / 4), 0.0f, room.position.z - (room.type.dimensions.z / 4)); //Spawn in bottom-left corner.
        enemy.GetComponent<Renderer>().material.color = Color.blue;

        enemy.transform.parent = room.getRoomBehaviour().transform.parent;
        enemy.SetActive(false);
        roomObjects.Add(enemy);
    }
}
