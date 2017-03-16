using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInventory : MonoBehaviour {

    List<Loot> lootInventory = new List<Loot>();

    public void addLoot(Loot newLoot)
    {
        lootInventory.Add(newLoot);
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
                break;
            }
        }
    }

    public void clearLoot()
    {
        lootInventory.Clear();
    }
}
