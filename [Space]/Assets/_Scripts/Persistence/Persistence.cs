﻿using space;
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
    public bool tutorialDone = false;
    List<String> weapons = new List<String>();
    List<int> consumables = new List<int>();
    List<int> currencies = new List<int>();
    List<String> heldWeapons = new List<string>();
    //int[] heldweapons = new int[4];
    List<GameObject> loot = new List<GameObject>();


    SceneArguments sceneArgs;

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
        data.tutorialDone = tutorialDone;

        foreach (var weapon in weapons)
        {
            if (!data.weapons.Contains(weapon))
            {
                data.weapons.Add(weapon);
            }
        }

        data.consumables.Clear();
        foreach (int consumable in consumables)
        {
            data.consumables.Add(consumable);
        }

        data.currencies.Clear();
        data.currencies.Add(Numbers.metals);
        data.currencies.Add(Numbers.organics);
        data.currencies.Add(Numbers.fuel);
        data.currencies.Add(Numbers.radioactive);
        //foreach (var currency in currencies)
        //{
        //    data.currencies.Add(currency);
        //}

        data.heldWeapons.Clear();
        foreach (var heldWeapon in heldWeapons)
        {
            data.heldWeapons.Add(heldWeapon);
        }

        data.loot.Clear();
        foreach (var lootItem in loot)
        {
            data.loot.Add(lootItem);
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
            tutorialDone = data.tutorialDone;
            
            foreach (var weapon in data.weapons)
            {
                weapons.Add(weapon);
            }

            foreach (int consumable in data.consumables)
            {
                consumables.Add(consumable);
            }
            FindObjectOfType<ConsumableInventory>().getConsumables(consumables);

            foreach (var currency in data.currencies)
            {
                currencies.Add(currency);
            }

            Numbers.metals = currencies[0];
            Numbers.organics = currencies[1];
            Numbers.fuel = currencies[2];
            Numbers.radioactive = currencies[3];

            foreach (var heldWeapon in data.heldWeapons)
            {
                heldWeapons.Add(heldWeapon);
                Debug.Log(heldWeapon);
            }
            FindObjectOfType<WeaponSlotWrapper>().getHeldWeapons(heldWeapons);

            foreach (var lootItem in data.loot)
            {
                loot.Add(lootItem);
            }
        }

        transferData();
    }


    public void getSaveableData()
    {
        loot = this.GetComponent<LootInventory>().getLoot();
        consumables = GameObject.FindObjectOfType<ConsumableInventory>().setConsumables();
        currencies = this.GetComponent<Currency>().getCurrency();
        weapons = this.GetComponent<Armoury>().getWeapons();
        heldWeapons = GameObject.FindObjectOfType<WeaponSlotWrapper>().setHeldWeapons();
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
        this.GetComponent<LootInventory>().setLoot(loot);
        //this.GetComponent<Currency>().setCurrency(currencies);
        this.GetComponent<Armoury>().setWeapons(weapons);    
    }

    public List<String> transferHeldWeapons()
    {
        return heldWeapons;
    }

    public List<int> transferConsumables()
    {
        return consumables;
    }


    public Dictionary<int, string> getSavedFiles()
    {
       Dictionary<int, string> tempDic = new Dictionary<int, string>();
       for (int i = 1; i < 5; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/SaveFile" + i + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/SaveFile" + i + ".dat", FileMode.Open);
                PlayerData data = (PlayerData)bf.Deserialize(file);
                file.Close();

                tempDic.Add(data.index, data.timestamp);
            }
            Debug.Log("Doesn't Exist: " + i);
        }
        return tempDic;
    }


    public static String getTimeStamp(DateTime value)
    {
        return value.ToString("yyyyMMdd HH:mm");
    }

    public void setSceneArgs(SceneArguments sceneArgs)
    {
        this.sceneArgs = sceneArgs;
    }

    public SceneArguments getSceneArgs()
    {
        return sceneArgs;
    }
}

[Serializable]
class PlayerData
{
    public int index;
    public String timestamp;
    public bool tutorialDone;
    public List<String> weapons = new List<String>();
    public List<int> consumables = new List<int>();
    public List<int> currencies = new List<int>();
    public List<String> heldWeapons = new List<string>();
    //public int[] heldweapons = new int[4];
    public List<GameObject> loot = new List<GameObject>();
}