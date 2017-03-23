using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

    public string name;
    public String prefabName;
    public int metalAmount;
    public int organicAmount;
    public int fuelAmount;
    public int radioactiveAmount;

    public Loot(string n, int metal, int organic, int fuel, int radioactive)
    {
        name = n;
        metalAmount = metal;
        organicAmount = organic;
        fuelAmount = fuel;
        radioactiveAmount = radioactive;
    }
}
