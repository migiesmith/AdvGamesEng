﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonGenerator : MonoBehaviour
{

    // The nubmer of rooms that will be generated as a set for connecting (not necesarily the room count)
    int SIZE = 100;

    // List containing all room types that can spawn
    List<RoomType> roomTypes;

    // The total weighting of the room types
    float roomWeightSum = 0.0f;

    // Reference to the path finding object
    WaypointPathfinder pathFinder;

    public UnityEvent onGenerated;

    public bool isGenerated = false;

    // Use this for initialization
    void Awake()
    {
        // Fill roomTypes with the room types to generate
        getRoomsTypes();

        // Create a root room
        Room root = new Room(roomTypes[Random.Range(0, roomTypes.Count)]);
        // Generate rooms out from the root
        generateDungeon(root);

        // From the root, find every room and create a GameObject
        createGameObjects(root);

        Debug.Log("Done Generating Dungeon.");

        setupWaypoints();

        onGenerated.Invoke();
        isGenerated = true;
    }

    public void setupWaypoints()
    {
        // Find a WaypointPathfinder
        pathFinder = gameObject.GetComponent<WaypointPathfinder>();
        WaypointNode[] nodes = GameObject.FindObjectsOfType<SpaceWaypointNode>();
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].ID = i;
        }
        // Set the WaypointPathfinder's Map
        pathFinder.Map = nodes;
        Debug.Log(nodes.Length +" nodes set as map.("+ pathFinder.Map.Length +")");
    }

    public Room generateDungeon(Room root)
    {
        // Create an array to store all rooms
        Room[] rooms = new Room[SIZE];
        rooms[0] = root;
        // Pick a random selection of rooms
        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room(pickRoomType());
        }

        // Store if each room has been used (default = false)
        bool[] used = new bool[SIZE];
        // Root is always first to be used
        used[0] = true;
        Room currRoom = root;
        // Keep track of the number of connections and iterations (stops the generation running too long)
        int connections = 0;
        int iterations = 0;
        while (connections < rooms.Length / 2 && iterations < rooms.Length * 300)
        {
            // Pick a random room to connect
            int index = Random.Range(1, rooms.Length);

            // Check if it has been used
            if (!used[index])
            {
                // Unused index, attempt to connect it
                // Pick a random side to try connect
                int conIdx = Random.Range(0, currRoom.connections.Length);
                // Connect the two rooms (returns true if successful, otherwise returns false)
                if (currRoom.connect(rooms[index], currRoom.connections[conIdx].direction))
                {
                    bool isOverlapping = false;
                    for (int i = 0; i < rooms.Length; i++)
                    {
                        if (i == index)
                            continue;

                        if (used[i] && rooms[index].overlaps(rooms[i]))
                        {
                            isOverlapping = true;
                            break;
                        }
                    }

                    if (!isOverlapping)
                    {// && inDungeonBounds(rooms[index]))
                        currRoom = rooms[index];
                        used[index] = true;
                        connections++;
                    }
                    else
                    {
                        currRoom.disconnect(rooms[index]);
                    }

                }
            }
            else
            {
                // The index was already used so set this 
                // as the current room to connect to
                currRoom = rooms[index];
            }

            iterations++;
        }

        // Loop through all rooms to find overlapping, unused, connections
        for (int ri = 0; ri < rooms.Length - 1; ri++)
        {
            if (!used[ri]) // Skip unused rooms
                continue;
            for (int rj = ri + 1; rj < rooms.Length; rj++)
            {
                if (!used[rj]) // Skip unused rooms
                    continue;
                for (int ci = 0; ci < rooms[ri].connections.Length; ci++)
                {
                    if (rooms[ri].connections[ci].connectedRoom == null)
                    { // If this connection is unused
                        for (int cj = 0; cj < rooms[rj].connections.Length; cj++)
                        {
                            if (rooms[rj].connections[cj].connectedRoom == null)
                            { // If this connection is unused
                                if (rooms[ri].position + rooms[ri].connections[ci].offset == rooms[rj].position + rooms[rj].connections[cj].offset
                                        && rooms[ri].connections[ci].direction * -1 == rooms[rj].connections[cj].direction)
                                {
                                    // Connect the rooms together
                                    rooms[ri].connections[ci].connectedRoom = rooms[rj];
                                    rooms[rj].connections[cj].connectedRoom = rooms[ri];
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        return root;
    }

    void createGameObjects(Room root)
    {
        // Create game objects
        List<Room> seen = new List<Room>();
        List<Room> toSee = new List<Room>();

        toSee.Add(root);
        while (toSee.Count > 0)
        {
            Room next = toSee[0];
            toSee.RemoveAt(0);
            seen.Add(next);
            for (int i = 0; i < next.connections.Length; i++)
            {
                if (next.connections[i].connectedRoom != null && !seen.Contains(next.connections[i].connectedRoom) && !toSee.Contains(next.connections[i].connectedRoom))
                {
                    toSee.Add(next.connections[i].connectedRoom);
                }
            }

            var go = new GameObject("Room");
            go.SetActive(false);
            RoomBehaviour rmBehav = go.AddComponent<RoomBehaviour>();
            rmBehav.room = next;
            go.transform.parent = this.transform;
            go.SetActive(true);

            onGenerated.AddListener(new UnityAction(delegate{rmBehav.hide();}));

        }

        this.transform.position = new Vector3(0, 0, 0);
    }

    RoomType pickRoomType()
    {
        float weight = Random.Range(0, this.roomWeightSum);
        foreach (RoomType type in this.roomTypes)
        {
            weight -= type.weighting;
            if (weight <= 0.0f)
                return type;
        }
        return roomTypes[Random.Range(0, this.roomTypes.Count)];
    }

    void getRoomsTypes()
    {
        // Add type to list
        roomTypes = new List<RoomType>();
        roomTypes.Add(new BasicRoomType());
        roomTypes.Add(new MedBayCornerRoomType());
        roomTypes.Add(new HydroponicsRoomType());

        roomWeightSum = 0.0f;
        foreach (RoomType type in roomTypes)
        {
            roomWeightSum += type.weighting;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
