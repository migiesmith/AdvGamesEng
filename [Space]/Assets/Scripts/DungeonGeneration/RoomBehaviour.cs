using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour {

    public Room room;


    // Use this for initialization
    void Start() {
        // Set the behaviour of my room
        this.room.setRoomBehaviour(this);
        this.tag = "teleportable";

        // Set my position
        this.transform.position = this.room.position;
        // Add Collider
        addPlane(this.room.type.dimensions.x, this.room.type.dimensions.z, false);
        // Add Room Model
        addRoomModel();

		for(int i = 0; i < this.transform.childCount; i++){
		    this.transform.GetChild(i).gameObject.SetActive(false);
		}

        // TODO remove, this is for showing valid connections
        for(int i = 0; i < this.room.connections.Length; i++){
            if(this.room.connections[i].connectedRoom != null){
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = this.transform.position + this.room.connections[i].offset;
            }
        }
        
    }

    // Update is called once per frame
    void Update() {

    }

    void addRoomModel(){      
        string modelName;
        float rotY = this.room.getOrientationAndModel(out modelName);

        GameObject model = (GameObject)Instantiate(Resources.Load("Prefabs/"+modelName));
        model.transform.parent = this.transform;
        model.transform.localPosition = new Vector3(0, 0, 0);
        model.transform.Rotate(new Vector3(0, rotY, 0));

        List<Connection> doors = this.room.getDoors();
        for(int i = 0; i < doors.Count; i++){
            Vector3 spawnPos = this.transform.position + doors[i].offset;
            if (!Physics.Raycast(spawnPos, new Vector3(0, 1, 0), 1.0f)) {
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
         new Vector3(-width/2, 0.0f, -height/2),
         new Vector3(width/2, 0.0f, -height/2),
         new Vector3(width/2, 0.0f, height/2),
         new Vector3(-width/2, 0.0f, height/2)
     };
        planeMesh.uv = new Vector2[] {
         new Vector2 (0, 0),
         new Vector2 (0, 1),
         new Vector2 (1, 1),
         new Vector2 (1, 0)
     };
        planeMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        planeMesh.RecalculateNormals();

        if(toRender){
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
