using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{

    // The room this behaviour represents
    public Room room;
    public RoomLayout layout;

    // Use this for initialization
    void Awake()
    {
        // Set the behaviour of my room
        this.room.setRoomBehaviour(this);
        this.tag = "teleportable";

        // Set my position
        this.transform.position = this.room.position;
        // Add Collider
        addPlane(this.room.type.dimensions.x, this.room.type.dimensions.z, false);
        // Add Room Model
        addRoomModel();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        /* TODO remove, this is for showing valid connections
        for(int i = 0; i < this.room.connections.Length; i++){
            if(this.room.connections[i].connectedRoom != null){
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = this.transform.position + this.room.connections[i].offset;
            }
        }
        */
    }

    void Start()
    {
        determineLayout();
        generateLoot(false);
        setupWaypoints();
        removeGenerationAreas();
    }

    void determineLayout()
    {
        int randomL = 0;
        switch (randomL)
        {
            case 0:
                layout = new BasicLayout(room);
                break;
            default:
                layout = new RandomLayout(room);
                break;
        }
    }

    void generateLoot(bool fromChest)
    {
        //TODO Connor

        int randomNum;
        int randomAmount;

        int roomsizeModifier = 0;
        //Makes it so loot is dropped more often on larger rooms.
        if (room.type.name == "Large Room")
        {
            roomsizeModifier = 20;
        }
        else if (room.type.name == "xLarge Room")
        {
            roomsizeModifier = 40;
        }

        //List of potential loot objects - Dictionary<loot name, chance to drop)
        Dictionary<string, int[]> lootTypes = new Dictionary<string, int[]>();
        lootTypes.Add("Money", new int[] { 40, 60 + roomsizeModifier });
        lootTypes.Add("MedKit", new int[] { 51, 70 + roomsizeModifier });
        lootTypes.Add("Other", new int[] { 71, 90 + roomsizeModifier });

        randomNum = Random.Range(0, 101);
        foreach (KeyValuePair<string, int[]> loot in lootTypes)
        {
            if (randomNum > loot.Value[0] && randomNum < loot.Value[1])
            {
                //Spawn Loot Item
                GameObject lootDrop = GameObject.CreatePrimitive(PrimitiveType.Cube);
                lootDrop.transform.parent = this.transform;
                lootDrop.SetActive(false);

                List<Transform> lootAreas = this.transform.FindDeepChildren("LootArea");
                if (lootAreas.Count > 0)
                {
                    lootDrop.transform.position = lootAreas[Random.Range(0, lootAreas.Count)].position; //Spawn in random loot area
                    if (fromChest == true)
                    {
                        //Spawn in front of user/ in front of chest
                        //lootDrop.transform.position = layout.roomObjects[0].transform.position;
                    }
                    lootDrop.GetComponent<Renderer>().material.color = Color.blue;

                    Debug.Log("Spawning Loot: " + loot.Key);
                    //Determine how much money is dropped.
                    if (loot.Key == "Money")
                    {
                        lootDrop.GetComponent<Renderer>().material.color = Color.yellow;
                        randomAmount = Random.Range(20, 1001);
                        int amount = randomAmount; //+ Any Modifiers -- Size, Difficulty etc.
                    }
                    else if (loot.Key == "MedKit") //Determine if small or large medkit is dropped.
                    {
                        randomAmount = Random.Range(0, 2);
                        if (randomAmount == 0)
                        {
                            //Spawn small medkit.
                            lootDrop.GetComponent<Renderer>().material.color = Color.red;
                            lootDrop.transform.localScale -= new Vector3(0, 0.5f, 0);
                        }
                        else
                        {
                            //Spawn large medkit.
                            lootDrop.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                    //Add any other instances of objects requiring more randomisation here.
                }
            }
        }

    }

    // Finds all children of this room that are 'LootAreas' and removes them (only call when they are no longer needed!)
    void removeGenerationAreas()
    {
        // Find all loot areas
        List<Transform> lootAreas = this.transform.FindDeepChildren("LootArea");
        // Loop through the results and Destroy them
        for (int i = 0; i < lootAreas.Count; i++)
        {
            Destroy(lootAreas[i].gameObject);
        }
    }

    // Checks for overlapping SpaceWaypointNodes and merges them
    void setupWaypoints()
    {
        // Get all waypoints in this room
        SpaceWaypointNode[] nodes = this.GetComponentsInChildren<SpaceWaypointNode>(true);
        // Loop through each waypoint
        foreach (SpaceWaypointNode node in nodes)
        {
            if (!node.enabled)
                continue;
            // Loop through the rooms connected to this
            foreach (Connection c in room.connections)
            {
                // Skip non-existant rooms
                if (c.connectedRoom == null)
                    continue;
                // Get the nodes of the connected room
                SpaceWaypointNode[] otherNodes = c.connectedRoom.getRoomBehaviour().GetComponentsInChildren<SpaceWaypointNode>(true);
                // Loop through and check for overlap
                foreach (SpaceWaypointNode otherNode in otherNodes)
                {
                    if (Vector3.Distance(node.transform.position, otherNode.transform.position) < 0.05f)
                    {
                        // There is overlap so add the otherNode's neighbors to this
                        node.neighbors.AddRange(otherNode.neighbors);
                        // Make the otherNode's connections point back to this
                        foreach (SpaceWaypointNode otherConNode in otherNode.neighbors)
                        {
                            otherConNode.neighbors.Add(node);
                            otherConNode.neighbors.Remove(otherNode);
                        }
                        // Destroy the overlapping node (otherNode)
                        Destroy(otherNode.gameObject);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void addRoomModel()
    {
        string modelName;
        float rotY = this.room.getOrientationAndModel(out modelName);

        GameObject model = (GameObject)Instantiate(Resources.Load("Prefabs/" + modelName));
        model.transform.parent = this.transform;
        model.transform.localPosition = new Vector3(0, 0, 0);
        model.transform.Rotate(new Vector3(0, rotY, 0));

        List<Connection> doors = this.room.getDoors();
        for (int i = 0; i < doors.Count; i++)
        {
            Vector3 spawnPos = this.transform.position + doors[i].offset;
            if (!Physics.Raycast(spawnPos, new Vector3(0, 1, 0), 1.0f))
            {
                GameObject door = (GameObject)Instantiate(Resources.Load("Prefabs/Door"));
                door.transform.position = spawnPos;
                door.transform.LookAt(door.transform.position - doors[i].direction);
                door.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
            }
        }
    }

    // TODO Remove this
    void addPlane(float width, float height, bool toRender)
    {
        Mesh planeMesh = new Mesh();
        planeMesh.name = "Quad";
        planeMesh.vertices = new Vector3[] {
         new Vector3(-width/2, -0.1f, -height/2),
         new Vector3(width/2, -0.1f, -height/2),
         new Vector3(width/2, -0.1f, height/2),
         new Vector3(-width/2, -0.1f, height/2)
     };
        planeMesh.uv = new Vector2[] {
         new Vector2 (0, 0),
         new Vector2 (0, 1),
         new Vector2 (1, 1),
         new Vector2 (1, 0)
     };
        planeMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        planeMesh.RecalculateNormals();

        if (toRender)
        {
            GameObject plane = new GameObject("Floor");
            plane.transform.parent = this.transform;
            plane.transform.position = this.room.position;
            MeshFilter meshFilter = plane.AddComponent<MeshFilter>();
            meshFilter.mesh = planeMesh;
            MeshRenderer renderer = plane.AddComponent<MeshRenderer>();
            renderer.material.shader = Shader.Find("Particles/Additive");
            Texture2D tex = new Texture2D(1, 1);
            Color col = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            tex.SetPixel(0, 0, col);
            tex.Apply();
            renderer.material.mainTexture = tex;
            renderer.material.color = col;
        }

        MeshCollider collider = this.gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = planeMesh;
        collider.transform.Rotate(new Vector3(180.0f, 0, 0));
    }

}
