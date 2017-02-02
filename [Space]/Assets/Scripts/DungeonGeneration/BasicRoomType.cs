using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRoomType :  RoomType{

    static class DIRECTION{
        public static int NORTH = 0;
        public static int EAST = 1;
        public static int SOUTH = 2;
        public static int WEST = 3;
    }

	public BasicRoomType(){
        // Basic Room Connections
        List<Connection> basicRoomConnections = new List<Connection>();
        basicRoomConnections.Add(new Connection(new Vector3(3.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)));
        basicRoomConnections.Add(new Connection(new Vector3(-3.0f, 0.0f, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f)));
        basicRoomConnections.Add(new Connection(new Vector3(0.0f, 0.0f, 3.0f), new Vector3(0.0f, 0.0f, 1.0f)));
        basicRoomConnections.Add(new Connection(new Vector3(0.0f, 0.0f, -3.0f), new Vector3(0.0f, 0.0f, -1.0f)));

		setParams(basicRoomConnections, new Vector3(6.0f, 6.0f, 6.0f));

        this.name = "Basic Room";
	}

    public override void getUsedDirections(Connection[] inConnections, out bool[] usedDirs, out int usedConnections){
        usedConnections = 0;   
        usedDirs = new bool[4]; // Up, Right, Down, Left
        
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

    }

	public override float getOrientationAndModel(Connection[] inConnections, out string modelName){
        int usedConnections;
        bool[] usedDirs;
        float rotY = 0.0f;


        getUsedDirections(inConnections, out usedDirs, out usedConnections);

        modelName = "CorridorCrossroads";

        if(usedConnections == 1){

        }else if(usedConnections == 2){
            if(usedDirs[DIRECTION.NORTH] && usedDirs[DIRECTION.EAST]){
                modelName = "CorridorCorner";
                rotY = 270.0f;
            }else if(usedDirs[DIRECTION.SOUTH] && usedDirs[DIRECTION.EAST]){
                modelName = "CorridorCorner";
            }else if(usedDirs[DIRECTION.SOUTH] && usedDirs[DIRECTION.WEST]){
                modelName = "CorridorCorner";
                rotY = 90.0f;
            }else if(usedDirs[DIRECTION.NORTH] && usedDirs[DIRECTION.WEST]){
                modelName = "CorridorCorner";
                rotY = 180.0f;
            }else if(usedDirs[DIRECTION.NORTH] && usedDirs[DIRECTION.SOUTH]){
                modelName = "CorridorStraight";
            }else if(usedDirs[DIRECTION.WEST] && usedDirs[DIRECTION.EAST]){
                modelName = "CorridorStraight";
                rotY = 90.0f;
            }
            
        }else if(usedConnections == 3){
            if(usedDirs[DIRECTION.WEST] && usedDirs[DIRECTION.NORTH] && usedDirs[DIRECTION.EAST]){
                modelName = "CorridorTJunction";
            }else if(usedDirs[DIRECTION.NORTH] && usedDirs[DIRECTION.EAST] && usedDirs[DIRECTION.SOUTH]){
                modelName = "CorridorTJunction";
                rotY = 90.0f;
            }else if(usedDirs[DIRECTION.EAST] && usedDirs[DIRECTION.SOUTH] && usedDirs[DIRECTION.WEST]){
                modelName = "CorridorTJunction";
                rotY = 180.0f;
            }else if(usedDirs[DIRECTION.SOUTH] && usedDirs[DIRECTION.WEST] && usedDirs[DIRECTION.NORTH]){
                modelName = "CorridorTJunction";
                rotY = 270.0f;
            }
        }
        
		return rotY;
	}

    public override List<Connection> getDoors(Connection[] inConnections){
        return new List<Connection>();
        
        bool[] usedDirs;
        int usedConnections;
        List<Connection> doors = new List<Connection>();

        getUsedDirections(inConnections, out usedDirs, out usedConnections);

        if(usedConnections > 1){
            return doors;
        }

        for(int i = 0; i < inConnections.Length; i++){
            if(inConnections[i].connectedRoom != null){
                doors.Add(inConnections[i]);
            }
        }
        return doors;
        
    }

}
