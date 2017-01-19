using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    Room root;

	// Use this for initialization
	void Start () {
        List<Connection> connections = new List<Connection>();
        connections.Add(new Connection(new Vector3(0.5f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)));
        connections.Add(new Connection(new Vector3(-0.5f, 0.0f, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f)));

        RoomType basicRoom = new RoomType(connections, new Vector3(1.0f, 1.0f, 1.0f));

        List<Room> rooms = new List<Room>();

        root = new Room(basicRoom, new Vector3(0, 1, 0));
        rooms.Add(root);
        Room room2 = new Room(basicRoom, new Vector3(0, 1, 0));
        root.connect(room2, room2.connections[0].direction);
        rooms.Add(room2);

        /*
        foreach (Room r in rooms)
        {
        }
        */

        List<Room> seen = new List<Room>();
        List<Room> toSee = new List<Room>();

        toSee.Add(root);
        while (toSee.Count > 0) {
            Room next = toSee[0];
            toSee.RemoveAt(0);
            for (int i = 0; i < next.connections.Count; i++) {
                Debug.Log(!Object.Equals(next.connections[i].connectedRoom, null));
                if (!Object.Equals(next.connections[i].connectedRoom, null) && !seen.Contains(next.connections[i].connectedRoom)) {
                    toSee.Add(next.connections[i].connectedRoom);
                    Debug.Log("Added");
                }else{
                    Debug.Log(next.connections[i].connectedRoom);
                }
            }

            var go = new GameObject();
            Room rCopy = go.AddComponent<Room>();
            rCopy.type = next.type;
            rCopy.connections = next.connections;
            rCopy.position = next.position;


            seen.Add(next);
        }


        Debug.Log("Done Generating.");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
