using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot {

    [SerializeField] public string name;
    [SerializeField] public string prefabName;
    public int metalAmount;
    public int organicAmount;
    public int fuelAmount;
    public int radioactiveAmount;

    public Loot()
    {

    }

    public Loot(string n, string prefab, int metal, int organic, int fuel, int radioactive)
    {
        name = n;
        prefabName = prefab;
        metalAmount = metal;
        organicAmount = organic;
        fuelAmount = fuel;
        radioactiveAmount = radioactive;
    }
}
