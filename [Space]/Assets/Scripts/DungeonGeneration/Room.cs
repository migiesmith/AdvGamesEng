/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

    // The RoomType associated with this Room
    public RoomType type;
    // Reference to the connections defined in this room's type
    public Connection[] connections { get { return this.type.connections; } set { this.type.connections = value; } }
    // The X, Y, and Z location of this room
    public Vector3 position;

    // The RoomBehaviour that owns this room, null until a GameObject is made for it
    RoomBehaviour roomBehaviour = null;

    // Constructor taking a type of room and a location
    public Room(RoomType type, Vector3 position = default(Vector3))
    {
        // Create a copy of the room type
        this.type = (RoomType)Activator.CreateInstance(type.GetType());
        // Set the position       
        this.position = position;
        // Randomise the orientation of this room's type
        this.type.randomiseOrientation();
    }

    // Returns true if the two rooms overlap on the X and/or Z axis
    public bool overlaps(Room r)
    {
        return !(
            r.position.x - r.type.dimensions.x / 2.0f + 0.0001f > this.position.x + this.type.dimensions.x / 2.0f || // X-Axis - Left v Right
            r.position.x + r.type.dimensions.x / 2.0f - 0.0001f < this.position.x - this.type.dimensions.x / 2.0f || // X-Axis - Right v Left
            r.position.z - r.type.dimensions.z / 2.0f + 0.0001f > this.position.z + this.type.dimensions.z / 2.0f || // Z-Axis - Top v Bottom
            r.position.z + r.type.dimensions.z / 2.0f - 0.0001f < this.position.z - this.type.dimensions.z / 2.0f);  // Z-Axis - Bottom v Top
    }

    // Get the index of an unused connection matching the direction dir
    int getConnection(Vector3 dir)
    {
        // Loop through each connection
        for (int i = 0; i < connections.Length; i++)
        {
            // If the connection is unused and the directions match then return i
            if (connections[i].direction == dir && connections[i].connectedRoom == null)
            {
                return i;
            }
        }
        // Couldn't find a matching connection
        return -1;
    }

    // Set the RoomBehaviour of this Room (should be used when assigned to a RoomBehaviour)
    public void setRoomBehaviour(RoomBehaviour roomBehaviour)
    {
        this.roomBehaviour = roomBehaviour;
    }

    // Get a reference to the RoomBehaviour assigned to this Room
    public RoomBehaviour getRoomBehaviour()
    {
        return this.roomBehaviour;
    }

    /*  Connect the incoming room to a door on this room
        Returns false if a connection cannot be made, true
        if a connection is sucessfully made.
    */
    public bool connect(Room toConnect, Vector3 direction)
    {
        // If the rooms are equal then this connection is not allowed
        if (this.Equals(toConnect))
            return false;

        // Get the opposite direction of the incoming direction
        Vector3 oppDir = direction * -1;

        // Get the index of the connections for each room
        int myIdx = getConnection(direction);
        int otherIdx = toConnect.getConnection(oppDir);

        // If either room has no valid connection then they cannot be connected
        if (myIdx == -1 || otherIdx == -1)
            return false;

        // This is a valid connection, move the incoming room toa valid location
        toConnect.position = this.position + this.connections[myIdx].offset - toConnect.connections[otherIdx].offset;

        // Assign the connections of each room
        toConnect.connections[otherIdx].connectedRoom = this;
        connections[myIdx].connectedRoom = toConnect;

        // Sucessfully connected rooms
        return true;
    }

    // Getter for the orientation and model defined by the room type (Simplifies Access)
    public float getOrientationAndModel(out string modelName)
    {
        return this.type.getOrientationAndModel(this.connections, out modelName);
    }

    // Getter for the used connections (doors) defined by the room type (Simplifies Access)
    public List<Connection> getDoors()
    {
        return this.type.getDoors(this.connections);
    }

    // Removes all connections between two rooms
    public void disconnect(Room toDisconnect)
    {
        // Look for a connection from toDisconnect
        for (int i = 0; i < connections.Length; i++)
        {
            if (toDisconnect.Equals(connections[i].connectedRoom))
            {
                // Remove the connection to toDisconnect
                connections[i].connectedRoom = null;
                // Remove toDisconnect's connection to this
                for (int j = 0; j < toDisconnect.connections.Length; j++)
                {
                    if (this.Equals(toDisconnect.connections[j].connectedRoom))
                    {
                        toDisconnect.connections[j].connectedRoom = null;
                    }
                }
            }
        }

    }


}
