using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    Room root;
    int SIZE = 200;

    WaypointPathfinder pathFinder;

	// Use this for initialization
	void Awake () {

        List<RoomType> rmTypes = getRoomsTypes();

        Room root = new Room(rmTypes[Random.Range(0, rmTypes.Count)]);
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

    List<RoomType> getRoomsTypes(){
        // Add type to list
        List<RoomType> rmTypes = new List<RoomType>();
        // TODO percentages
        rmTypes.Add(new BasicRoomType());
        rmTypes.Add(new BasicRoomType());
        rmTypes.Add(new BasicRoomType());
        rmTypes.Add(new BasicRoomType());
        rmTypes.Add(new MedBayCornerRoomType());
        rmTypes.Add(new HydroponicsRoomType());

        return rmTypes;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
