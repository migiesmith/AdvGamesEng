using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    public RoomType type;
    public Connection[] connections;
    public Vector3 position;

    RoomBehaviour roomBehaviour = null;

    public Room(RoomType type, Vector3 position = default(Vector3)) {
        this.type = (RoomType)Activator.CreateInstance(type.GetType());        
        this.position = position;

        this.type.randomiseOrientation();
        Debug.Log("T: " + this.type.orientation);

        connections = new Connection[this.type.connections.Count];
        for(int i = 0; i < connections.Length; i++) {
            connections[i] = new Connection(this.type.connections[i].offset, this.type.connections[i].direction);
        }

    }

    
    public bool overlaps(Room r) {
        return!(
            r.position.x - r.type.dimensions.x / 2.0f + 0.0001f > this.position.x + this.type.dimensions.x / 2.0f ||
            r.position.x + r.type.dimensions.x / 2.0f - 0.0001f < this.position.x - this.type.dimensions.x / 2.0f ||
            r.position.z - r.type.dimensions.z / 2.0f + 0.0001f > this.position.z + this.type.dimensions.z / 2.0f ||
            r.position.z + r.type.dimensions.z / 2.0f - 0.0001f < this.position.z - this.type.dimensions.z / 2.0f);
    }
    

    int getConnection(Vector3 dir) {
        for(int i = 0; i < connections.Length; i++) {
            if (connections[i].direction == dir && connections[i].connectedRoom == null) {
                return i;
            }
        }
        return -1;
    }

    public void setRoomBehaviour(RoomBehaviour roomBehaviour){
        this.roomBehaviour = roomBehaviour;
    }
    public RoomBehaviour getRoomBehaviour(){
        return this.roomBehaviour;
    }

    public bool connect(Room toConnect, Vector3 direction) {
        
        if (this.Equals(toConnect))
            return false;
        

        Vector3 oppDir = direction * -1;

        int myIdx = getConnection(direction);
        int otherIdx = toConnect.getConnection(oppDir);        

        if (myIdx == -1 || otherIdx == -1)
            return false;
        
        toConnect.position = this.position + this.connections[myIdx].offset - toConnect.connections[otherIdx].offset;
        toConnect.connections[otherIdx].connectedRoom = this;
        
        connections[myIdx].connectedRoom = toConnect;  

        return true;
    }

    public float getOrientationAndModel(out string modelName){
        return this.type.getOrientationAndModel(this.connections, out modelName);
    }

    public List<Connection> getDoors(){
        return this.type.getDoors(this.connections);
    }

    public void disconnect(Room toDisconnect) {        
        // Look for a connection from toDisconnect
        for(int i = 0; i < connections.Length; i++) {
            if (toDisconnect.Equals(connections[i].connectedRoom)) {
                // Remove the connection to toDisconnect
                connections[i].connectedRoom = null;
                // Remove toDisconnect's connection to this
                for (int j = 0; j < toDisconnect.connections.Length; j++) {
                    if (this.Equals(toDisconnect.connections[j].connectedRoom)) {
                        toDisconnect.connections[j].connectedRoom = null;
                    }
                }
            }
        }
        
    }


}
