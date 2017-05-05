/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroponicsRoomType : RoomType
{

    public const int NUM_DIRECTIONS = 2;


    public HydroponicsRoomType()
    {
        // Basic Room Connections
        List<Connection> connections = new List<Connection>();
        connections.Add(new Connection(new Vector3(0.0f, 0.0f, 14.4f), new Vector3(0.0f, 0.0f, 1.0f))); // North
        connections.Add(new Connection(new Vector3(0.0f, 0.0f, -14.4f), new Vector3(0.0f, 0.0f, -1.0f))); // South


        setParams(connections, new Vector3(20.0f, 12.0f, 28.8f), 0.15f);

        this.name = "Hydroponics";

        priority = 2;
    }

    public override int getPriority()
    {
        int usedDoors = 0;
        foreach (Connection c in connections)
            if (c.connectedRoom != null)
                usedDoors++;
        return priority - usedDoors;
    }

    // Overrides RoomType's implementation as this room is unaffected by rotation
    public override void randomiseOrientation()
    {
        float angle = Random.Range(0.0f, 360.0f);
        angle -= angle % 180.0f;
        setOrientation(angle);

        if (angle % 180.0f != 0.0f)
        {
            this.dimensions = new Vector3(28.8f, 12.0f, 20.0f);
        }
    }

    // Gets an array defining what rooms have been used
    public override void getUsedDirections(Connection[] inConnections, out bool[] usedDirs, out int usedConnections)
    {
        usedConnections = 0;
        usedDirs = new bool[NUM_DIRECTIONS];


        /*
        for(int i = 0; i < inConnections.Length; i++){
            if(inConnections[i].connectedRoom != null){
                usedConnections++;
                if(inConnections[i].direction == new Vector3(1,0,0)){
                    usedDirs[1] = true;
                }else if(inConnections[i].direction == new Vector3(-1,0,0)){
                    usedDirs[3] = true;
                }else if(inConnections[i].direction == new Vector3(0,0,-1)){
                    usedDirs[2] = true;
                }else if(inConnections[i].direction == new Vector3(0,0,1)){
                    usedDirs[0] = true;
                }
            }
		}
		*/

        usedConnections = 2;
    }

    // Returns a float defining the orientation of the room, and passes the modelName back through an inputted variable
    public override float getOrientationAndModel(Connection[] inConnections, out string modelName)
    {
        int usedConnections;
        bool[] usedDirs;
        float rotY = orientation;

        getUsedDirections(inConnections, out usedDirs, out usedConnections);

        modelName = "Hydroponics";

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
    }

}
