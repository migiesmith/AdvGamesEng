/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{

    // The room this behaviour represents
    public Room room;
    public RoomLayout layout;

    private DungeonParams param;

    public List<Light> lights = new List<Light>();

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

        /* TODO remove, this is for showing valid connections
        for(int i = 0; i < this.room.connections.Length; i++){
            if(this.room.connections[i].connectedRoom != null){
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = this.transform.position + this.room.connections[i].offset;
            }
        }
        */

        Light[] childLights = GetComponentsInChildren<Light>();
        for (int i = 0; i < childLights.Length; ++i)
        {
            childLights[i].gameObject.SetActive(false);
            lights.Add(childLights[i]);
        }
    }

    void Start()
    {
        // Generate items in the room
        ItemGeneration itemGen = new ItemGeneration(this);
        itemGen.generate();

        setupWaypoints();

        // Generate enemies in the room
        EnemyGeneration enemyGen = new EnemyGeneration(this);
        enemyGen.generate();

        removeGenerationAreas();
    }

    void setVisibility(bool isVisible)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(isVisible);
        }
    }

    public void hide()
    {
        setVisibility(false);
    }

    public void show()
    {
        setVisibility(true);
    }

    void determineLayout()
    {
        int randomL = 1;//Random.Range(0, 2);
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

    // Finds all children of this room that are 'LootAreas' and removes them (only call when they are no longer needed!)
    void removeGenerationAreas()
    {
        // Find all loot areas
        List<Transform> lootAreas = this.transform.FindDeepChildren("LootAreas");
        // Loop through the results and Destroy them
        for (int i = 0; i < lootAreas.Count; i++)
        {
            Destroy(lootAreas[i].gameObject);
        }

        // Find all loot areas
        lootAreas = this.transform.FindDeepChildren("LootArea");
        // Loop through the results and Destroy them
        for (int i = 0; i < lootAreas.Count; i++)
        {
            Destroy(lootAreas[i].gameObject);
        }


        // Find all enemy areas
        List<Transform> enemyAreas = this.transform.FindDeepChildren("EnemyAreas");
        // Loop through the results and Destroy them
        for (int i = 0; i < enemyAreas.Count; i++)
        {
            Destroy(enemyAreas[i].gameObject);
        }

        // Find all enemy areas
        enemyAreas = this.transform.FindDeepChildren("EnemyArea");
        // Loop through the results and Destroy them
        for (int i = 0; i < enemyAreas.Count; i++)
        {
            Destroy(enemyAreas[i].gameObject);
        }

    }

    // Checks for overlapping SpaceWaypointNodes and merges them
    void setupWaypoints()
    {
        // Get all waypoints in this room
        SpaceWaypointNode[] nodes = this.GetComponentsInChildren<SpaceWaypointNode>(true);

        // Loop through each waypoint
        for (int i = 0; i < nodes.Length; ++i)
        {
            nodes[i].setPosition();
            if (!nodes[i].enabled)
                continue;
            // Loop through the rooms connected to this
            for (int idx = 0; idx < room.connections.Length; ++idx)
            {
                // Skip non-existant rooms
                if (room.connections[idx].connectedRoom == null)
                    continue;
                // Get the nodes of the connected room
                SpaceWaypointNode[] otherNodes = room.connections[idx].connectedRoom.getRoomBehaviour().GetComponentsInChildren<SpaceWaypointNode>(true);
                // Loop through and check for overlap
                for (int otherIdx = 0; otherIdx < otherNodes.Length; ++otherIdx)
                {
                    if (Vector3.Distance(nodes[i].transform.position, otherNodes[otherIdx].transform.position) < 0.05f)
                    {
                        // There is overlap so add the otherNode's neighbors to this
                        nodes[i].neighbors.AddRange(otherNodes[otherIdx].neighbors);
                        // Make the otherNode's connections point back to this
                        for (int otherConIdx = 0; otherConIdx < otherNodes[otherIdx].neighbors.Count; ++otherConIdx)
                        {
                            otherNodes[otherIdx].neighbors[otherConIdx].neighbors.Add(nodes[i]);
                            otherNodes[otherIdx].neighbors[otherConIdx].neighbors.Remove(otherNodes[otherIdx]);
                        }
                        // Destroy the overlapping node (otherNode)
                        Destroy(otherNodes[otherIdx].gameObject);
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

        RoomDatabase roomDB = GameObject.FindObjectOfType<RoomDatabase>();
        if (roomDB != null)
        {
            GameObject model = (GameObject)Instantiate(roomDB.getRoom(modelName));
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



    public void setParams(DungeonParams param)
    {
        this.param = param;
    }
    public DungeonParams getParams()
    {
        return param;
    }

}
