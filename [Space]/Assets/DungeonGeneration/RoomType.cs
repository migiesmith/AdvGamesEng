using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class RoomType {

    public List<Connection> connections;
    public Vector3 dimensions;

    public RoomType(List<Connection> connections, Vector3 dimensions) {
        this.connections = connections;
        this.dimensions = dimensions;
    }

}
