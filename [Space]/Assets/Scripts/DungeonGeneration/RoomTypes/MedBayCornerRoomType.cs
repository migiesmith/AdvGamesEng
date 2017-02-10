using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedBayCornerRoomType :  RoomType{

    static class DIRECTION{
        public static int NORTH = 0;
        public static int EAST = 1;
    }

    public const int NUM_DIRECTIONS = 2;


	public MedBayCornerRoomType(){
        // Basic Room Connections
        List<Connection> connections = new List<Connection>();
        connections.Add(new Connection(new Vector3(6.0f, 0.0f, -3.0f), new Vector3(1.0f, 0.0f, 0.0f))); // East
        connections.Add(new Connection(new Vector3(-3.0f, 0.0f, 6.0f), new Vector3(0.0f, 0.0f, 1.0f))); // North


		setParams(connections, new Vector3(12.0f, 6.0f, 12.0f), 0.4f);

        this.name = "Medbay Corner";
	}


    public override void getUsedDirections(Connection[] inConnections, out bool[] usedDirs, out int usedConnections){
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

	public override float getOrientationAndModel(Connection[] inConnections, out string modelName){
        int usedConnections;
        bool[] usedDirs;
        float rotY = orientation;
        Debug.Log("Med: "+rotY);

        getUsedDirections(inConnections, out usedDirs, out usedConnections);

        modelName = "MedbayCorner";
        
		return rotY;
	}

    public override List<Connection> getDoors(Connection[] inConnections){
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
