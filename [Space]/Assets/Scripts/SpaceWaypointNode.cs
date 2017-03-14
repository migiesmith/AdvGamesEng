using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceWaypointNode : WaypointNode
{

    // Use this for initialization
    void Start()
    {
        setPosition();
    }

    public void setPosition()
    {
        this.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {        
        SpaceWaypointPathfinder pather = GameObject.FindObjectOfType<SpaceWaypointPathfinder>();
        if(pather != null)
        {
            WaypointNode[] newMap = new WaypointNode[pather.Map.Length - 1];
            int index = 0;
            for(int i = 0; i < pather.Map.Length; i++)
            {
                if(pather.Map[i] != this)
                {
                    newMap[index] = pather.Map[i];
                    newMap[index].ID = index;
                    index++;
                }
            }
            pather.Map = newMap;
        }
    }
}
