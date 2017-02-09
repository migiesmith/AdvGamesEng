using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unique layout used for a Randomised room.
public class RandomLayout : RoomLayout
{
    public RandomLayout(Room r) : base(r) 
    {
        generateLights();
        generateRoomObjects();
    }

    public override void generateLights()
    {
        int max = 4;
    }

    public override void generateRoomObjects()
    {

    }
}
