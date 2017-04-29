/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration
{

    private RoomBehaviour roomBehav;

    public EnemyGeneration(RoomBehaviour rb)
    {
        this.roomBehav = rb;
    }

    public DungeonParams getParams()
    {
        return roomBehav.getParams();
    }

    public void generate()
    {
        List<Transform> enemyAreas = roomBehav.transform.FindDeepChildren("EnemyArea");
        for (int i = 0; i < enemyAreas.Count; ++i)
        {
            if (Random.Range(0.0f, 1.0f) < getParams().enemySpawnRate)
            {
                Enemy toSpawn = pickEnemy();
                if (toSpawn != null)
                {
                    GameObject go = null;
                    if (toSpawn != null)
                    {
                        go = (GameObject)GameObject.Instantiate(toSpawn).gameObject;
                        go.name = toSpawn.name;
                    }
                    else
                    {
                        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        go.name = "Enemy Should be here";
                    }
                    go.transform.position = enemyAreas[i].position;
                    go.name = toSpawn.name;
                }
            }
        }
    }

    Enemy pickEnemy()
    {
        DungeonParams param = getParams();
        if (param != null)
        {
            float totalWeight = 0.0f;
            for (int i = 0; i < param.enemies.Count; ++i)
            {
                totalWeight += param.enemies[i].weight;
            }

            totalWeight *= Random.Range(0.0f, 1.0f);
            for (int i = 0; i < param.enemies.Count; ++i)
            {
                totalWeight -= param.enemies[i].weight;
                if (totalWeight <= 0.0f)
                    return param.enemies[i].enemy;
            }
            if (param.enemies.Count > 0)
            {
                return param.enemies[Random.Range(0, param.enemies.Count)].enemy;
            }
        }
        return null;
    }


}
