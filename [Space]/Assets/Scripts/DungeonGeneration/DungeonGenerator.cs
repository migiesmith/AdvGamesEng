using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    Room root;
    int SIZE = 100;

	// Use this for initialization
	void Start () {
        // Basic Room Connections
        List<Connection> basicRoomConnections = new List<Connection>();
        basicRoomConnections.Add(new Connection(new Vector3(1f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)));
        basicRoomConnections.Add(new Connection(new Vector3(-1f, 0.0f, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f)));
        basicRoomConnections.Add(new Connection(new Vector3(0.0f, 0.0f, 1f), new Vector3(0.0f, 0.0f, 1.0f)));
        basicRoomConnections.Add(new Connection(new Vector3(0.0f, 0.0f, -1f), new Vector3(0.0f, 0.0f, -1.0f)));
        // Basic Room Type
        RoomType basicRoom = new RoomType(basicRoomConnections, new Vector3(2.0f, 2.0f, 2.0f));

        // Large Room Connections
        List<Connection> largeRoomConnections = new List<Connection>();
        largeRoomConnections.Add(new Connection(new Vector3(2.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)));
        largeRoomConnections.Add(new Connection(new Vector3(-2.0f, 0.0f, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f)));
        largeRoomConnections.Add(new Connection(new Vector3(0.0f, 0.0f, 2.0f), new Vector3(0.0f, 0.0f, 1.0f)));
        largeRoomConnections.Add(new Connection(new Vector3(0.0f, 0.0f, -2.0f), new Vector3(0.0f, 0.0f, -1.0f)));
        // Large Room Type
        RoomType largeRoom = new RoomType(largeRoomConnections, new Vector3(4.0f, 4.0f, 4.0f));

        // X-Axis Hall Connections
        List<Connection> xAxisHallConnections = new List<Connection>();
        xAxisHallConnections.Add(new Connection(new Vector3(2.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)));
        xAxisHallConnections.Add(new Connection(new Vector3(-2.0f, 0.0f, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f)));
        // X-Axis Hall Room Type
        RoomType xAxisHall = new RoomType(xAxisHallConnections, new Vector3(4.0f, 2.0f, 2.0f));

        // Z-Axis Hall Connections
        List<Connection> zAxisHallConnections = new List<Connection>();
        zAxisHallConnections.Add(new Connection(new Vector3(0.0f, 0.0f, 2.0f), new Vector3(0.0f, 0.0f, 1.0f)));
        zAxisHallConnections.Add(new Connection(new Vector3(0.0f, 0.0f, -2.0f), new Vector3(0.0f, 0.0f, -1.0f)));
        // Z-Axis Hall Room Type
        RoomType zAxisHall = new RoomType(zAxisHallConnections, new Vector3(2.0f, 2.0f, 4.0f));

        // Large Room Connections
        List<Connection> xlargeRoomConnections = new List<Connection>();
        xlargeRoomConnections.Add(new Connection(new Vector3(3.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)));
        xlargeRoomConnections.Add(new Connection(new Vector3(-3.0f, 0.0f, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f)));
        xlargeRoomConnections.Add(new Connection(new Vector3(0.0f, 0.0f, 3.0f), new Vector3(0.0f, 0.0f, 1.0f)));
        xlargeRoomConnections.Add(new Connection(new Vector3(0.0f, 0.0f, -3.0f), new Vector3(0.0f, 0.0f, -1.0f)));
        // Large Room Type
        RoomType xLargeRoom = new RoomType(xlargeRoomConnections, new Vector3(6.0f, 6.0f, 6.0f));

        // Add type to list
        List<RoomType> rmTypes = new List<RoomType>();
        rmTypes.Add(basicRoom);
        rmTypes.Add(xAxisHall);
        rmTypes.Add(zAxisHall);
        rmTypes.Add(largeRoom);
        rmTypes.Add(xLargeRoom);


        Room root = new Room(basicRoom);
        generateDungeon(root, rmTypes);
        
    
      
    // Create game objects
    List<Room> seen = new List<Room>();
        List<Room> toSee = new List<Room>();

        toSee.Add(root);
        while (toSee.Count > 0) {
            Room next = toSee[0];
            toSee.RemoveAt(0);
            for (int i = 0; i < next.connections.Length; i++)
            {
                if (next.connections[i].connectedRoom != null && !seen.Contains(next.connections[i].connectedRoom)) {
                    toSee.Add(next.connections[i].connectedRoom);
                }
            }

            var go = new GameObject("Room");
            RoomBehaviour rmBehav = go.AddComponent<RoomBehaviour>();
            rmBehav.room = next;
            go.transform.parent = this.transform;

            seen.Add(next);
        }

        this.transform.position = new Vector3(0, 0, 0);

        Debug.Log("Done Generating.");
        
    }
	
    public Room generateDungeon(Room root, List<RoomType> roomTypes){
        // Make Rooms
        Room[] rooms = new Room[SIZE];
        rooms[0] = root;
        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room(roomTypes[i % roomTypes.Count]);
        }


        
        bool[] used = new bool[SIZE];
        used[0] = true;
        Room currRoom = root;
        int connections = 0;
        int iterations = 0;
        while (connections < rooms.Length / 2 && iterations < rooms.Length * 300)
        {
            int index = Random.Range(1, rooms.Length);

            if (!used[index]) {
                int conIdx = Random.Range(0, currRoom.connections.Length);
                if (currRoom.connect(rooms[index], currRoom.connections[conIdx].direction)) {
                    bool isOverlapping = false;
                    for (int i = 0; i < rooms.Length; i++){
                        if (i == index)
                            continue;

                        if (used[i] && rooms[index].overlaps(rooms[i]))
                        {
                            isOverlapping = true;
                            break;
                        }
                    }
                    
                    if (!isOverlapping) {// && inDungeonBounds(rooms[index]))
                        currRoom = rooms[index];
                        used[index] = true;
                        connections++;
                    } else {
                        currRoom.disconnect(rooms[index]);
                    }
                    
                }
            }
            else
            {
                currRoom = rooms[index];
            }

            iterations++;
        }

        return root;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
