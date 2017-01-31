using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour {

    public Room room;


    // Use this for initialization
    void Start() {
        this.transform.position = this.room.position;
        addPlane(this.room.type.dimensions.x, this.room.type.dimensions.z, false);
        addRoomModel();
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
        int usedConnections = 0;
        bool posX = false, negX = false, posZ = false, negZ = false;
        for(int i = 0; i < this.room.connections.Length; i++){
            if(this.room.connections[i].connectedRoom != null){
                usedConnections++;
                if(this.room.connections[i].direction == new Vector3(1,0,0)){
                    posX = true;
                }else if(this.room.connections[i].direction == new Vector3(-1,0,0)){
                    negX = true;
                }else if(this.room.connections[i].direction == new Vector3(0,0,1)){
                    posZ = true;
                }else if(this.room.connections[i].direction == new Vector3(0,0,-1)){
                    negZ = true;

                }
            }
        }

        if(usedConnections == 4 && this.room.type.dimensions.x == 6.0f){
            GameObject model = (GameObject)Instantiate(Resources.Load("Prefabs/CorridorCrossroads"));
            model.transform.parent = this.transform;
            model.transform.localPosition = new Vector3(0, 0, 0);
        }else if(usedConnections == 2 && ((posX && negZ) || (negX && posZ))){
            GameObject model = (GameObject)Instantiate(Resources.Load("Prefabs/CorridorWallCorner"));
            model.transform.parent = this.transform;
            model.transform.localPosition = new Vector3(0, 0, 0);
            if(posX && negZ){

            }else if(negX && posZ){
                model.transform.Rotate(new Vector3(0, 180.0f, 0));
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
