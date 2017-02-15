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

            int randomNum;
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
            Dictionary<string, int[]> lootTypes = new Dictionary<string, int[]>();
            lootTypes.Add("Money", new int[] { 40, 60 + roomsizeModifier });
            lootTypes.Add("MedKit", new int[] { 51, 70 + roomsizeModifier });
            lootTypes.Add("Other", new int[] { 71, 90 + roomsizeModifier });

            randomNum = Random.Range(0, 101);
            foreach (KeyValuePair<string, int[]> loot in lootTypes)
            {
                if (randomNum > loot.Value[0] && randomNum < loot.Value[1])
                {
                    //Spawn Loot Item
                    GameObject lootDrop = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    lootDrop.transform.parent = roomBehaviour.transform;
                    lootDrop.SetActive(false);

                    List<Transform> lootAreas = roomBehaviour.transform.FindDeepChildren("LootArea");
                    if (lootAreas.Count > 0)
                    {
                        lootDrop.transform.position = lootAreas[Random.Range(0, lootAreas.Count)].position; //Spawn in random loot area
                        if (fromChest == true)
                        {
                            //Spawn in front of user/ in front of chest
                            //lootDrop.transform.position = layout.roomObjects[0].transform.position;
                        }
                        lootDrop.GetComponent<Renderer>().material.color = Color.blue;

                        Debug.Log("Spawning Loot: " + loot.Key);
                        //Determine how much money is dropped.
                        if (loot.Key == "Money")
                        {
                            lootDrop.GetComponent<Renderer>().material.color = Color.yellow;
                            randomAmount = Random.Range(20, 1001);
                            int amount = randomAmount; //+ Any Modifiers -- Size, Difficulty etc.
                        }
                        else if (loot.Key == "MedKit") //Determine if small or large medkit is dropped.
                        {
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
                        }
                        //Add any other instances of objects requiring more randomisation here.
                    }
                }
            }

        }
    }
