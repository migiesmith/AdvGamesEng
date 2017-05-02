using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armoury : MonoBehaviour {

    //Try purely adding weapons to the armoury only with prebaf names and not with the GameObject method.
    Dictionary<String, int> weapons = new Dictionary<String, int>();
    public space.PrefabDatabase prefabs;

    /*public void createWeaponByIndex(int index, Transform trans) //Doesn't work with dictionary.
    {
        Instantiate(prefabs.getPrefab(weapons[index]), trans);
        weapons.RemoveAt(index);
    }*/

    
    public void CreateWeaponByName(String name, Transform trans)
    {
        if (weapons.ContainsKey(name))
        {
            Instantiate(prefabs.getPrefab(name), trans);
            if(weapons[name] == 1)
            {
                weapons.Remove(name);
            }
            else
            {
                weapons[name] -= 1;
            }
        }
    }

    public void addWeapon(GameObject weapon)
    {
        //Get GameObject prefab name;

        if (weapons.ContainsKey(weapon.name)) //Weapon.name may not be the prefab name.
        {
            weapons[weapon.name] += 1;
        }
        else
        {
            weapons.Add(weapon.name, 1);
        }

        Destroy(weapon);
    }

    public void addWeapon(String prefabName)
    {
        if (weapons.ContainsKey(prefabName))
        {
            weapons[prefabName] += 1;
        }
        else
        {
            weapons.Add(prefabName, 1);
        }
    }
  
      
    public Dictionary<String, int> getWeapons()
    {
        return weapons;
    }

    public void setWeapons(Dictionary<String, int> weapon)
    {
        weapons = weapon;
    }
}
