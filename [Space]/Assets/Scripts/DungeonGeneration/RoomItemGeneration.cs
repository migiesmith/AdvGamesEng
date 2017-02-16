using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RoomItemGeneration : MonoBehaviour
{

    Room room;
    RoomBehaviour roomBehaviour;

    public RoomItemGeneration(Room r, RoomBehaviour rb)
    {
        this.room = r;
        this.roomBehaviour = rb;
        generateLoot(false);
    }

    void generateLoot(bool fromChest)
    {
        //TODO Connor

        int randomAmount;
        int roomsizeModifier = 0;
        //Makes it so loot is dropped more often on larger rooms.
        if (room.type.name == "Large Room")
        {
            roomsizeModifier = 20;
        }
        else if (room.type.name == "xLarge Room")
        {
            roomsizeModifier = 40;
        }

        //List of potential loot objects - Dictionary<loot name, chance to drop)
        Dictionary<string, int> lootTypes = new Dictionary<string, int>();
        lootTypes.Add("Money", 60 + roomsizeModifier);
        lootTypes.Add("MedKit", 30 + roomsizeModifier);
        lootTypes.Add("Other", 20 + roomsizeModifier);


        List<Transform> lootAreas = room.getRoomBehaviour().transform.FindDeepChildren("lootArea");
        foreach (Transform lootArea in lootAreas)
        {
            int count = 0;

            foreach (KeyValuePair<string, int> value in lootTypes)
            {
                count += value.Value;
            }

            int randomNum = Random.Range(0, count + 1);

            foreach (KeyValuePair<string, int> loot in lootTypes)
            {
                randomNum -= loot.Value;
                if (randomNum <= 0)
                {
                    //Generate Item at location.
                    GameObject lootDrop = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    lootDrop.transform.position = lootArea.position;
                    lootDrop.transform.parent = room.getRoomBehaviour().transform.parent;
                    lootDrop.SetActive(false);
                    Debug.Log("Spawning Loot: " + loot.Key);
                    switch (loot.Key)
                    {
                        case "Money":
                            lootDrop.GetComponent<Renderer>().material.color = Color.yellow;
                            randomAmount = Random.Range(20, 1001);
                            int amount = randomAmount; //+ Any Modifiers -- Size, Difficulty etc.
                            break;
                        case "MedKit":

                            randomAmount = Random.Range(0, 2);
                            if (randomAmount == 0)
                            {
                                //Spawn small medkit.
                                lootDrop.GetComponent<Renderer>().material.color = Color.red;
                                lootDrop.transform.localScale -= new Vector3(0, 0.5f, 0);
                            }
                            else
                            {
                                //Spawn large medkit.
                                lootDrop.GetComponent<Renderer>().material.color = Color.red;
                            }
                            break;
                        case "Other":
                            //Other Stuff here
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
