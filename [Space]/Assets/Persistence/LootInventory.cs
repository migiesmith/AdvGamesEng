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
            if(lootItem.getName() == name)
            {
                lootInventory.Remove(lootItem);
                lootAmount -= 1;
                break;
            }
        }
    }

    public void clearLoot()
    {
        lootInventory.Clear();
    }
}
