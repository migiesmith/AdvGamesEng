using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour {

    public Room room;


    // Use this for initialization
    void Start() {
        this.transform.position = this.room.position;
        addPlane(this.room.type.dimensions.x, this.room.type.dimensions.y);
    }

    // Update is called once per frame
    void Update() {

    }
    
    // TODO Remove this
    void addPlane(float width, float height)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {
         new Vector3(-width/2, 0.0f, -height/2),
         new Vector3(width/2, 0.0f, -height/2),
         new Vector3(width/2, 0.0f, height/2),
         new Vector3(-width/2, 0.0f, height/2)
     };
        m.uv = new Vector2[] {
         new Vector2 (0, 0),
         new Vector2 (0, 1),
         new Vector2 (1, 1),
         new Vector2 (1, 0)
     };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();


        GameObject plane = new GameObject("Plane");
        plane.transform.parent = this.transform;
        plane.transform.position = this.room.position;
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = m;
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material.shader = Shader.Find("Particles/Additive");
        Texture2D tex = new Texture2D(1, 1);
        Color col = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        tex.SetPixel(0, 0, col);
        tex.Apply();
        renderer.material.mainTexture = tex;
        renderer.material.color = col;
    }

}
