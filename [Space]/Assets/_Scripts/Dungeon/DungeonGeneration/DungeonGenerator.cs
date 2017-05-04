/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CielaSpike;

[RequireComponent (typeof (RoomDatabase))]
public class DungeonGenerator : MonoBehaviour
{


    // List containing all room types that can spawn
    List<RoomType> roomTypes;

    // The total weighting of the room types
    float roomWeightSum = 0.0f;

    // Reference to the path finding object
    WaypointPathfinder pathFinder;

    public UnityEvent onGenerated;

    public bool isGenerated = false;

    [Header("Generation Parameters")]
    public DungeonParams dgnParams;

    // Use this for initialization
    void Awake()
    {
        // Check if persistence has scene args for us
        Persistence persistence = GameObject.FindObjectOfType<Persistence>();
        if(persistence != null)
        {
            SceneArguments sceneArgs = persistence.getSceneArgs();
            // Check if the scene args are for dungeon generation
            if(sceneArgs != null && sceneArgs is DungeonArgs)
            {
                DungeonArgs dgnArgs = (DungeonArgs) sceneArgs;
                dgnParams = dgnArgs.getDgnParams();
            }
        }else{Debug.Log("no persistence");}

        StartCoroutine("startGeneration");
    }

    IEnumerator startGeneration()
    {
        // Fill roomTypes with the room types to generate
        getRoomsTypes();

        // Create a root room
        Room root = new Room(new AirLockRoomType());//roomTypes[Random.Range(0, roomTypes.Count)]);
        // Generate rooms out from the root
        generateDungeon(root);


        yield return Ninja.JumpToUnity;
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

    }

    public Room generateDungeon(Room root)
    {
        // Create an array to store all rooms
        Room[] rooms = new Room[dgnParams.RoomPoolSize];
        rooms[0] = root;
        // Pick a random selection of rooms
        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room(pickRoomType());
        }

        // Store if each room has been used (default = false)
        bool[] used = new bool[dgnParams.RoomPoolSize];
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

                }else{
                    // Reroll the room as it failed to connect
                    rooms[index] = new Room(pickRoomType());
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
                                    if(Random.Range(0.0f, 1.0f) < dgnParams.adjacentConnectChance || rooms[ri].type.getPriority() > 0 || rooms[rj].type.getPriority() > 0)
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
        }

        float avgDist = 0.0f; // Used to determine where to spawn the objective
        for(int i = 0; i < rooms.Length; i++)
        {
            if(rooms[i].type.getPriority() > 0){
                Connection[] doors = rooms[i].connections;
                Room newRoom = new Room(pickRoomType());
                while(newRoom.type.getPriority() != 0){
                    newRoom = new Room(pickRoomType());
                }
                for(int j = 0; j < doors.Length; j++)
                {
                    if(rooms[i].connect(newRoom, doors[j].direction))
                    {
                        newRoom = new Room(pickRoomType());
                        while(newRoom.type.getPriority() != 0){
                            newRoom = new Room(pickRoomType());
                        }
                    }
                }
            }
            // Add the distance to the average (used for the objective spawn) 
            avgDist += Vector3.Distance(root.position, rooms[i].position);
        }

        avgDist /= rooms.Length;        
        for(int i = 0; i < rooms.Length; i++)
        {
            if(rooms[i].type is BasicRoomType && Vector3.Distance(root.position, rooms[i].position) > avgDist){                
                GameObject objective = (GameObject)Instantiate(dgnParams.objective);
                objective.name = dgnParams.objective.name;
                objective.transform.position = rooms[i].position + new Vector3(0.0f, 1.0f, 0.0f);
                break;
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
            rmBehav.setParams(dgnParams);
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
        for(int i = 0; i < this.roomTypes.Count; ++i)
        {
            weight -= this.roomTypes[i].weighting;
            if (weight <= 0.0f)
                return this.roomTypes[i];
        }
        return roomTypes[Random.Range(0, this.roomTypes.Count)];
    }

    void getRoomsTypes()
    {
        // Add type to list
        roomTypes = new List<RoomType>();
        roomTypes.Add(new BasicRoomType());
        //roomTypes.Add(new MedBayCornerRoomType());
        roomTypes.Add(new HydroponicsRoomType());
        roomTypes.Add(new CargoBayRoomType());

        roomWeightSum = 0.0f;
        for(int i = 0; i < this.roomTypes.Count; ++i)
        {
            roomWeightSum += this.roomTypes[i].weighting;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
