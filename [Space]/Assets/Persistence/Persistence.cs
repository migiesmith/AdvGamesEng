using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Persistence : MonoBehaviour
{

    public string username;
    public int level;
    public Dictionary<GameObject, int> weapons = new Dictionary<GameObject, int>();


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    public void saveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + username + ".dat", FileMode.Open);

        PlayerData data = new PlayerData();
        data.username = username;
        data.level = level;
        foreach (var weapon in weapons)
        {
            if (!data.weapons.ContainsKey(weapon.Key))
            {
                data.weapons.Add(weapon.Key, weapon.Value);
            }
        }

        bf.Serialize(file, data);
        file.Close();
    }

    public void loadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/" + username + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + username + ".dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            username = data.username;
            level = data.level;
            foreach (var weapon in data.weapons)
            {
                weapons.Add(weapon.Key, weapon.Value);
            }
        }
    }

    public void addWeapon()
    {
        //TODO When weapons are added in game to add them here.
    }


    public void newGame()
    {

    }


}

[Serializable]
class PlayerData
{
    public string username;
    public int level;
    public Dictionary<GameObject, int> weapons = new Dictionary<GameObject, int>();
}