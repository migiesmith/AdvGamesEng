/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLockRoomType : RoomType
{

    public const int NUM_DIRECTIONS = 1;

    public AirLockRoomType()
    {
        // Basic Room Connections
        List<Connection> connections = new List<Connection>();
        connections.Add(new Connection(new Vector3(3.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)));


        setParams(connections, new Vector3(6.0f, 6.0f, 6.0f), 1.0f);

        this.name = "Air Lock";		
    }

    public override int getPriority(){
        int usedDoors = 0;
        foreach(Connection c in connections)
            if(c.connectedRoom != null)
                usedDoors++;
        return priority - usedDoors;
    }

    // Sets the orientation, rotating the connections accordingly
    public override void getUsedDirections(Connection[] inConnections, out bool[] usedDirs, out int usedConnections)
    {
        usedConnections = 0;
        usedDirs = new bool[NUM_DIRECTIONS];
        usedConnections = 2;
    }

    // Returns a float defining the orientation of the room, and passes the modelName back through an inputted variable
    public override float getOrientationAndModel(Connection[] inConnections, out string modelName)
    {
        int usedConnections;
        bool[] usedDirs;
        float rotY = orientation;

        getUsedDirections(inConnections, out usedDirs, out usedConnections);

        modelName = "AirLock";

        return rotY;
    }

    // Returns a list of all used connections (doors)
    public override List<Connection> getDoors(Connection[] inConnections)
    {
        return new List<Connection>();

        bool[] usedDirs;
        int usedConnections;
        List<Connection> doors = new List<Connection>();

        getUsedDirections(inConnections, out usedDirs, out usedConnections);

        return doors;
        /*
        for(int i = 0; i < inConnections.Length; i++){
            if(inConnections[i].connectedRoom != null){
                doors.Add(inConnections[i]);
            }
        }
        return doors;
		*/
    }

}
