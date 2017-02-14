using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomLayout
{
    public Room room;

    public List<GameObject> roomObjects = new List<GameObject>();

    private Random random = new Random();

    public RoomLayout(Room r) 
    {
        this.room = r;
    }

    public abstract void generateRoomObjects();
}
