using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    Room root;
    int SIZE = 100;

    List<RoomType> roomTypes;
    float roomWeightSum = 0.0f;

    WaypointPathfinder pathFinder;


	// Use this for initialization
	void Awake () {

       getRoomsTypes();

        Room root = new Room(roomTypes[Random.Range(0, roomTypes.Count)]);
        generateDungeon(root);
        
    
      
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
            go.SetActive(false);
            RoomBehaviour rmBehav = go.AddComponent<RoomBehaviour>();
            rmBehav.room = next;
            go.transform.parent = this.transform;
            go.SetActive(true);

            seen.Add(next);
        }

        this.transform.position = new Vector3(0, 0, 0);

        Debug.Log("Done Generating.");
        

    }
	
    void Start(){ 
        StartCoroutine(LateStart(1));
     }
 
     IEnumerator LateStart(float waitTime)
     {
        yield return new WaitForSeconds(waitTime);

        pathFinder = gameObject.GetComponent<WaypointPathfinder>();
        WaypointNode[] nodes = GameObject.FindObjectsOfType<SpaceWaypointNode>();  
        for(int i = 0; i < nodes.Length; i++){
            nodes[i].ID = i;
        }      
        pathFinder.Map = nodes;
    }

    public Room generateDungeon(Room root){
        // Make Rooms
        Room[] rooms = new Room[SIZE];
        rooms[0] = root;
        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room(pickRoomType());
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

        /* TODO Fix this code, it for some reason creates duplicates
        
        // Loop through all rooms to find overlapping, unused, connections
        for(int ri = 0; ri < rooms.Length - 1; ri++){
            if(!used[ri]) // Skip unused rooms
                continue;
            for(int rj = ri + 1; rj < rooms.Length; rj++){
                if(!used[rj]) // Skip unused rooms
                    continue;
                for(int ci = 0; ci < rooms[ri].connections.Length; ci++){
                    if(rooms[ri].connections[ci].connectedRoom == null){ // If this connection is unused
                        for(int cj = 0; cj < rooms[rj].connections.Length; cj++){
                            if(rooms[rj].connections[cj].connectedRoom == null){ // If this connection is unused
                                if(rooms[ri].position + rooms[ri].connections[ci].offset == rooms[rj].position + rooms[rj].connections[cj].offset
                                        && rooms[ri].connections[ci].direction * -1 == rooms[rj].connections[cj].direction){
                                    // Connect the rooms together
                                    rooms[ri].connect(rooms[rj], rooms[ri].connections[ci].direction);//.connections[ci].connectedRoom = rooms[rj];
                                    //rooms[rj].connections[cj].connectedRoom = rooms[ri];
                                }
                            }                        
                        }
                    }
                }
            }
        }
        */

        return root;
    }

    RoomType pickRoomType(){
        float weight = Random.Range(0, this.roomWeightSum);
        foreach(RoomType type in this.roomTypes){
            weight -= type.weighting;
            if(weight <= 0.0f)
                return type;
        }
        return roomTypes[Random.Range(0, this.roomTypes.Count)];
    }

    void getRoomsTypes(){
        // Add type to list
        roomTypes = new List<RoomType>();
        roomTypes.Add(new BasicRoomType());
        roomTypes.Add(new MedBayCornerRoomType());
        roomTypes.Add(new HydroponicsRoomType());

        roomWeightSum = 0.0f;
        foreach(RoomType type in roomTypes){
            roomWeightSum += type.weighting;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
