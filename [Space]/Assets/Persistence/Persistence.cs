using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Persistence : MonoBehaviour
{

    public int index = 1;
    public String timestamp;
    List<String> weapons = new List<String>();
    Dictionary<String, int> consumables = new Dictionary<string, int>();
    Dictionary<String, int> currencies = new Dictionary<string, int>();
    List<String> heldWeapons = new List<string>();
    //int[] heldweapons = new int[4];
    Dictionary<String, List<int>> loot = new Dictionary<string, List<int>>();


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    public void saveGame()
    {
        getSaveableData();
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log(Application.persistentDataPath);

        FileStream file;
        if(File.Exists(Application.persistentDataPath + "/SaveFile" + index  + ".dat")){
            file = File.Open(Application.persistentDataPath + "/SaveFile" + index  + ".dat", FileMode.Open);
        }
        else{
            file = File.Open(Application.persistentDataPath + "/SaveFile" + index  + ".dat", FileMode.Create);
        }

        PlayerData data = new PlayerData();
        data.index = index;

        timestamp = getTimeStamp(DateTime.Now);
        data.timestamp = timestamp;

        foreach (var weapon in weapons)
        {
            if (!data.weapons.Contains(weapon))
            {
                data.weapons.Add(weapon);
            }
        }

        data.consumables.Clear();
        foreach (var consumable in consumables)
        {
            data.consumables.Add(consumable.Key, consumable.Value);
        }

        data.currencies.Clear();
        foreach (var currency in currencies)
        {
            data.currencies.Add(currency.Key, currency.Value);
        }

        data.heldWeapons.Clear();
        foreach (var heldWeapon in heldWeapons)
        {
            data.heldWeapons.Add(heldWeapon);
        }

        data.loot.Clear();
        foreach (var lootItem in loot)
        {
            data.loot.Add(lootItem.Key, lootItem.Value);
        }

        bf.Serialize(file, data);
        file.Close();
    }

    public void loadGame(int ind)
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFile" + ind + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFile" + ind + ".dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            index = ind;
            
            foreach (var weapon in data.weapons)
            {
                weapons.Add(weapon);
            }

            foreach (var consumable in data.consumables)
            {
                consumables.Add(consumable.Key, consumable.Value);
            }

            foreach (var currency in data.currencies)
            {
                currencies.Add(currency.Key, currency.Value);
            }

            foreach (var heldWeapon in data.heldWeapons)
            {
                heldWeapons.Add(heldWeapon);
            }

            foreach (var lootItem in data.loot)
            {
                loot.Add(lootItem.Key, lootItem.Value);
            }
        }

        transferData();
    }


    public void getSaveableData()
    {
        //TODO get data to be saved.
    }


    public bool newGame()
    {
        for(int i = 1; i < 5; i++)
        {
            if (!File.Exists(Application.persistentDataPath + "/SaveFile" + i + ".dat"))
            {
                index = i;
                return true;
            }
        }
        return false;
    }


    public void deleteSaveFile(int ind)
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFile" + ind + ".dat"))
        {
            File.Delete(Application.persistentDataPath + "/SaveFile" + ind + ".dat");
        }
    }


    public void transferData()
    {
        //TODO Other Transfers
    }


    public static String getTimeStamp(DateTime value)
    {
        return value.ToString("yyyyMMdd HH:mm");
    }
}

[Serializable]
class PlayerData
{
    public int index;
    public String timestamp;
    public List<String> weapons = new List<String>();
    public Dictionary<String, int> consumables = new Dictionary<string, int>();
    public Dictionary<String, int> currencies = new Dictionary<string, int>();
    public List<String> heldWeapons = new List<string>();
    //public int[] heldweapons = new int[4];
    public Dictionary<String, List<int>> loot = new Dictionary<string, List<int>>();
}