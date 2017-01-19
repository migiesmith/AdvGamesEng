using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public RoomType type;
    public List<Connection> connections;
    public Vector3 position;


    public Room(RoomType type, Vector3 position = default(Vector3)) {
        this.type = type;
        this.position = position;
        connections = new List<Connection>();
        foreach(Connection c in type.connections){
            connections.Add(new Connection(c.offset, c.direction));
        }
    }
    bool overlaps(Room r) {
        return !(r.transform.position.x - r.type.dimensions.x / 2 > this.transform.position.x + this.type.dimensions.x / 2 ||
          r.transform.position.x + r.type.dimensions.x / 2 < this.transform.position.x - this.type.dimensions.x / 2 ||
          r.transform.position.y - r.type.dimensions.y / 2 > this.transform.position.y + this.type.dimensions.y / 2 ||
          r.transform.position.y + r.type.dimensions.y / 2 < this.transform.position.y - this.type.dimensions.y / 2);
    }

    int getConnection(Vector3 dir) {
        for(int i = 0; i < connections.Count; i++) {
            if (connections[i].direction == dir && connections[i].connectedRoom == null) {
                return i;
            }
        }
        return -1;
    }

    public bool connect(Room toConnect, Vector3 direction) {
        /*
        if (Object.Equals(this, toConnect))
            return false;
        */

        Vector3 oppDir = direction * -1;

        int myIdx = getConnection(direction);
        int otherIdx = toConnect.getConnection(oppDir);        

        if (myIdx != -1 && otherIdx != -1) {
            Connection myCon = connections[myIdx];
            myCon.connectedRoom = toConnect;

            Connection otherCon = toConnect.connections[otherIdx];
            otherCon.connectedRoom = this;
            toConnect.position = this.position + myCon.offset;
        } else {
            return false;
        }       

        return true;
    }

    public void disconnect(Room toDisconnect) {
        // Look for a connection from toDisconnect
        for(int i = 0; i < connections.Count; i++) {
            Connection con = connections[i];
            if (Object.ReferenceEquals(toDisconnect, con.connectedRoom)) {
                // Remove the connection to toDisconnect
                con.connectedRoom = null;
                // Remove toDisconnect's connection to this
                for (int j = 0; j < toDisconnect.connections.Count; j++) {
                    Connection otherCon = toDisconnect.connections[j];
                    if (Object.ReferenceEquals(this, otherCon.connectedRoom)) {
                        otherCon.connectedRoom = null;
                    }
                }

            }
        }
    }

    // Use this for initialization
    void Start() {
        this.transform.position = position;
        addPlane(this.type.dimensions.x, this.type.dimensions.y);
    }

    // Update is called once per frame
    void Update() {

    }

    // TODO Remove this
    void addPlane(float width, float height) {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {
         new Vector3(-width/4, 0.0f, -height/4),
         new Vector3(width/4, 0.0f, -height/4),
         new Vector3(width/4, 0.0f, height/4),
         new Vector3(-width/4, 0.0f, height/4)
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
        plane.transform.position = this.position;
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = m;
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material.shader = Shader.Find("Particles/Additive");
        Texture2D tex = new Texture2D(1, 1);
        Color col = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)  );
        tex.SetPixel(0, 0, col);
        tex.Apply();
        renderer.material.mainTexture = tex;
        renderer.material.color = col;
    }

}
