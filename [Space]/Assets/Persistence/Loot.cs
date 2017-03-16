using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

    String name;
    int metalAmount;
    int organicAmount;
    int fuelAmount;
    int radioactiveAmount;

    public Loot(string n, int metal, int organic, int fuel, int radioactive)
    {
        name = n;
        metalAmount = metal;
        organicAmount = organic;
        fuelAmount = fuel;
        radioactiveAmount = radioactive;
    }

    public String getName()
    {
        return name;
    }
}
