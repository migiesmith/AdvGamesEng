/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneration
{

    [System.Serializable]
    public class ItemWeight
    {
        [SerializeField] public Item item;
        [SerializeField] public float weight;
    }

    private RoomBehaviour roomBehav;

    public ItemGeneration(RoomBehaviour rb)
    {
        this.roomBehav = rb;
    }

    public DungeonParams getParams()
    {
        return roomBehav.getParams();
    }

    public void generate()
    {
        List<Transform> lootAreas = roomBehav.transform.FindDeepChildren("LootArea");
        for (int i = 0; i < lootAreas.Count; ++i)
        {
            Item toSpawn = pickItem();
            if (toSpawn != null)
            {
                GameObject go = null;
                if (toSpawn.prefab != null)
                {
                    go = (GameObject)GameObject.Instantiate(toSpawn.prefab);
                    go.name = toSpawn.name;
                }
                else
                {
                    go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }
                go.transform.position = lootAreas[i].position + new Vector3(0,2,0);
                go.transform.rotation = Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)).normalized);
                go.name = toSpawn.name;
                go.transform.parent = roomBehav.transform;
            }
        }
    }

    Item pickItem()
    {
        DungeonParams param = getParams();
        if (param != null)
        {
            float totalWeight = 0.0f;
            for (int i = 0; i < param.items.Count; ++i)
            {
                totalWeight += param.items[i].weight;
            }

            totalWeight *= Random.Range(0.0f, 1.0f);
            for (int i = 0; i < param.items.Count; ++i)
            {
                totalWeight -= param.items[i].weight;
                if (totalWeight <= 0.0f)
                    return param.items[i].item;
            }
            if (param.items.Count > 0)
            {
                return param.items[Random.Range(0, param.items.Count)].item;
            }
        }
        return null;
    }

}
