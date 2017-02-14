using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unique layout used for a Randomised room.
public class RandomLayout : RoomLayout
{

    public RandomLayout(Room r) : base(r) 
    {
        generateRoomObjects();
    }

    public override void generateRoomObjects()
    {

        List<Transform> lootAreas = room.getRoomBehaviour().transform.FindDeepChildren("ObjectArea");
        foreach(Transform lootArea in lootAreas)
        {

        }

        Dictionary<string, int> objectTypes = new Dictionary<string, int>();
        objectTypes.Add("Chest", 20);
        objectTypes.Add("Table", 30);
        objectTypes.Add("Enemy", 30);
        objectTypes.Add("None", 60);

        int count = 0;

        foreach(KeyValuePair<string, int> value in objectTypes)
        {
            count += value.Value;
        }

        int iterations = lootAreas[0].localScale.x * lootAreas[0].localScale.z;   

        for(int i = 0; i < iterations; i++)
        {
            int randomNum = Random.Range(0, count + 1);
            foreach (KeyValuePair<string, int> dic in objectTypes)
            {
                count -= dic.Value;
                if(count <= 0)
                {
                    //Generate Item at location.
                    switch (dic.Key)
                    {
                        case "Chest":
                            GameObject chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            chest.GetComponent<Renderer>().material.color = Color.blue;

                            chest.transform.parent = room.getRoomBehaviour().transform.parent;
                            chest.SetActive(false);
                            roomObjects.Add(chest);
                            break;
                        case "Table":
                            GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            table.GetComponent<Renderer>().material.color = Color.red;

                            table.transform.parent = room.getRoomBehaviour().transform.parent;
                            table.SetActive(false);
                            roomObjects.Add(table);
                            break;
                        case "Enemy":
                            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            enemy.GetComponent<Renderer>().material.color = Color.blue;

                            enemy.transform.parent = room.getRoomBehaviour().transform.parent;
                            enemy.SetActive(false);
                            roomObjects.Add(enemy);
                            break;
                        default:                       
                            break;
                    }

                    if(dic.Key != "None")
                    {
                        //TODO Set the location based 
                        switch (i)
                        {
                            case 0:
                                roomObjects[roomObjects.Count - 1].transform.position
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            default:
                                break;
                        }
                    }
                 
                    break;
                }
            }
        }
    }
}
