using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Armoury : MonoBehaviour {

    List<String> weapons = new List<String>();
    public space.PrefabDatabase prefabs;

    public void createWeaponByIndex(int index, Transform trans)
    {
        Instantiate(prefabs.getPrefab(weapons[index]), trans);
        weapons.RemoveAt(index);
    }

    public void CreateWeaponByName(String name, Transform trans)
    {
        Instantiate(prefabs.getPrefab(name), trans);
        weapons.Remove(name);
    }

    public void addWeapon(GameObject weapon)
    {
        //Get GameObject prefab name;
        weapons.Add(weapon.name);
        Destroy(weapon);
    }

    public void addWeapon(String prefabName)
    {
        weapons.Add(prefabName);
    }
    
    public List<String> getWeapons()
    {
        return weapons;
    }

    public void setWeapons(List<String> weapon)
    {
        weapons = weapon;
    }
}
