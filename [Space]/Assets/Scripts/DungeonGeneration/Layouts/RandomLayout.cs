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

        Dictionary<string, int> objectTypes = new Dictionary<string, int>();
        objectTypes.Add("Chest", 20);
        objectTypes.Add("Table", 30);
        objectTypes.Add("Something", 30);
        objectTypes.Add("None", 60);

        Dictionary<string, int> enemyTypes = new Dictionary<string, int>();
        enemyTypes.Add("Normal", 70);
        enemyTypes.Add("Heavy", 30);

        //Generate Objects
        List<Transform> objectAreas = room.getRoomBehaviour().transform.FindDeepChildren("ObjectArea");
        foreach (Transform objectArea in objectAreas)
        {
            int count = 0;

            foreach (KeyValuePair<string, int> value in objectTypes)
            {
                count += value.Value;
            }

            int randomNum = Random.Range(0, count + 1);
            foreach (KeyValuePair<string, int> dic in objectTypes)
            {
                randomNum -= dic.Value;
                if (randomNum <= 0)
                {
                    //Generate Item at location.
                    switch (dic.Key)
                    {
                        case "Chest":
                            GameObject chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            chest.GetComponent<Renderer>().material.color = Color.blue;
                            chest.transform.position = objectArea.position;
                            chest.transform.parent = room.getRoomBehaviour().transform.parent;
                            chest.SetActive(false);
                            roomObjects.Add(chest);
                            break;
                        case "Table":
                            GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            table.GetComponent<Renderer>().material.color = Color.red;
                            table.transform.position = objectArea.position;
                            table.transform.parent = room.getRoomBehaviour().transform.parent;
                            table.SetActive(false);
                            roomObjects.Add(table);
                            break;
                        case "Something":
                            GameObject something = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            something.GetComponent<Renderer>().material.color = Color.blue;
                            something.transform.position = objectArea.position;
                            something.transform.parent = room.getRoomBehaviour().transform.parent;
                            something.SetActive(false);
                            roomObjects.Add(something);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        Debug.Log("HERE");

        //Generate Enemies.
        List<Transform> enemyAreas = room.getRoomBehaviour().transform.FindDeepChildren("EnemyArea");
        foreach (Transform enemyArea in enemyAreas)
        {
            int count = 0;

            foreach (KeyValuePair<string, int> value in enemyTypes)
            {
                count += value.Value;
            }

            int randomNum = Random.Range(0, count + 1);
            foreach (KeyValuePair<string, int> dic in enemyTypes)
            {
                randomNum -= dic.Value;
                if (randomNum <= 0)
                {
                    Debug.Log(dic.Key);
                    //Generate Item at location.
                    switch (dic.Key)
                    {
                        case "Normal":
                            //Create Normal Enemy
                            GameObject enemy = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Enemy"));
                            //roomObjects.Add(enemy);
                            break;
                        case "Heavy":
                            //Create Heavy Enemy
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
