using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInventory : MonoBehaviour {

    List<Loot> lootInventory = new List<Loot>();
    int lootAmount = 0;

    public bool addLoot(Loot newLoot)
    {
        if(lootAmount <= 10)
        {
            lootInventory.Add(newLoot);
            lootAmount += 1;
            return true;
        }
        return false;
    }

    public void setLoot(List<Loot> lootIn)
    {
        lootInventory = lootIn;
    }

    public List<Loot> getLoot()
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
        GameObject Loot = (GameObject)Instantiate(Resources.Load(lootInventory[index].prefabName));
        lootInventory.RemoveAt(index);
    }


    public void clearLoot()
    {
        lootInventory.Clear();
    }


    public void sellAll()
    {
        foreach(Loot loot in lootInventory)
        {
            this.GetComponent<Currency>().addCurrency(loot.metalAmount, loot.organicAmount, loot.fuelAmount, loot.radioactiveAmount);
            lootInventory.Remove(loot);
        }
    }
}
