using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInventory : MonoBehaviour {

    List<GameObject> lootInventory = new List<GameObject>();
    int lootAmount = 0;

    public bool addLoot(GameObject newLoot)
    {
        if(lootAmount <= 10)
        {
            lootInventory.Add(newLoot);
            lootAmount += 1;
            return true;
        }
        return false;
    }

    public void setLoot(List<GameObject> lootIn)
    {
        lootInventory = lootIn;
    }

    public List<GameObject> getLoot()
    {
        return lootInventory;
    }

    public void removeLoot(String name)
    {
        foreach (var lootItem in lootInventory)
        {
            if(lootItem.name == name)
            {
                lootInventory.Remove(lootItem);
                lootAmount -= 1;
                break;
            }
        }
    }


    public void dropLoot(int index)
    {
        //GameObject Loot = (GameObject)Instantiate(Resources.Load(lootInventory[index].prefabName));
        Instantiate(lootInventory[index]);
        lootInventory.RemoveAt(index);
    }


    public void clearLoot()
    {
        lootInventory.Clear();
    }


    public void sellAll()
    {
        foreach(GameObject loot in lootInventory)
        {
            ShopValues vals = loot.transform.root.GetComponent<ShopValues>();
            if (vals != null)
                vals.sell();

            //this.GetComponent<Currency>().addCurrency(loot.metalAmount, loot.organicAmount, loot.fuelAmount, loot.radioactiveAmount);
            lootInventory.Remove(loot);
        }
    }
}
