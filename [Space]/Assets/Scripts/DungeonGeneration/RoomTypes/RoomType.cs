/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections.Generic;
using UnityEngine;

// Abstract class defining the specifications of a room
public abstract class RoomType : System.Object
{

    // Name of the room type
    public string name;
    // The connections of the room type
    public Connection[] connections;
    // The X, Y, and Z size of the room type
    public Vector3 dimensions;

    // The weighting associated with this room type
    public float weighting = 1.0f;

    // The orientation of this room type
    public float orientation = 0.0f;

    // Empty constructor
    public RoomType() { }

    // Sets the connections, dimensions, and weighting of this room type
    public void setParams(List<Connection> connections, Vector3 dimensions, float weighting)
    {
        // Copy the connections
        this.connections = new Connection[connections.Count];
        for (int i = 0; i < connections.Count; i++)
        {
            this.connections[i] = new Connection(connections[i].offset, connections[i].direction);
        }

        // Set the dimensions and weighting
        this.dimensions = dimensions;
        this.weighting = weighting;
    }

    // Selects a random orientation 
    public virtual void randomiseOrientation()
    {
        float angle = Random.Range(0.0f, 360.0f);
        angle -= angle % 90.0f;
        setOrientation(angle);
    }

    // Sets the orientation, rotating the connections accordingly
    protected void setOrientation(float angle)
    {
        this.orientation = angle;
        Connection[] newConnections = new Connection[this.connections.Length];
        // Rotate each connections
        for (int i = 0; i < newConnections.Length; i++)
        {
            newConnections[i] = new Connection(
                Quaternion.AngleAxis(orientation, new Vector3(0.0f, 1.0f, 0.0f)) * this.connections[i].offset,
                Quaternion.AngleAxis(orientation, new Vector3(0.0f, 1.0f, 0.0f)) * this.connections[i].direction
            );
        }
        // Update the connections
        this.connections = newConnections;
    }

    // Gets an array defining what rooms have been used
    public abstract void getUsedDirections(Connection[] inConnections, out bool[] usedDirs, out int usedConnections);
    // Returns a float defining the orientation of the room, and passes the modelName back through an inputted variable
    public abstract float getOrientationAndModel(Connection[] connections, out string modelName);
    // Returns a list of all used connections (doors)
    public abstract List<Connection> getDoors(Connection[] inConnections);

}

// Defines the offset and direction of a room type's connection
public struct Connection
{
    // Offset from room's center
    public Vector3 offset;
    // Direction the connection faces
    public Vector3 direction;
    // The room using this connection, null if there is not one
    public Room connectedRoom;

    public Connection(Vector3 offset, Vector3 direction)
    {
        this.offset = offset;
        this.direction = direction;
        connectedRoom = null;
    }

}
