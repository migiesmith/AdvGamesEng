using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Armoury : MonoBehaviour {

    List<String> weapons = new List<String>();

    public void createWeaponByIndex(int index, Transform trans)
    {
        GameObject prefab = (GameObject)Instantiate(Resources.Load(weapons[index]));
        GameObject weapon = GameObject.Instantiate(prefab, trans);
        weapons.RemoveAt(index);
    }

    public void CreateWeaponByName(String name, Transform trans)
    {
        GameObject prefab = (GameObject)Instantiate(Resources.Load(name));
        GameObject weapon = GameObject.Instantiate(prefab, trans);
        weapons.Remove(name);
    }

    public void addWeapon(GameObject weapon)
    {
        //Get GameObject prefab name;
        UnityEngine.Object prefab = PrefabUtility.GetPrefabParent(weapon);
        weapons.Add(AssetDatabase.GetAssetPath(prefab));
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
