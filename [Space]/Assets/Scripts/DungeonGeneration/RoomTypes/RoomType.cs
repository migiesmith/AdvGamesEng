using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RoomType : System.Object {

    public string name;
    public List<Connection> connections;
    public Vector3 dimensions;

    public float weighting = 1.0f;

    public float orientation = 0.0f;

    public RoomType(){

    }

    public void setParams (List<Connection> connections, Vector3 dimensions, float weighting) {
        this.connections = connections;
        this.dimensions = dimensions;
        this.weighting = weighting;
    }

    public virtual void randomiseOrientation(){
        float angle = Random.Range(0.0f, 360.0f);
        angle -= angle % 90.0f;
        setOrientation(angle);
    }

    protected void setOrientation(float angle){
        this.orientation = angle;
        List<Connection> newConnections = new List<Connection>();
        foreach(Connection con in this.connections){
            newConnections.Add(new Connection(
                Quaternion.AngleAxis(orientation, new Vector3(0.0f, 1.0f, 0.0f)) * con.offset, 
                Quaternion.AngleAxis(orientation, new Vector3(0.0f, 1.0f, 0.0f)) * con.direction
            ));
        }        
        this.connections = newConnections;
    }

    public abstract void getUsedDirections(Connection[] inConnections, out bool[] usedDirs, out int usedConnections);
    public abstract float getOrientationAndModel(Connection[] connections, out string modelName);

    public abstract List<Connection> getDoors(Connection[] inConnections);

}


public struct Connection {
    public Vector3 offset;
    public Vector3 direction;
    public Room connectedRoom;

    public Connection(Vector3 offset, Vector3 direction) {
        this.offset = offset;
        this.direction = direction;
        connectedRoom = null;
    }

}
