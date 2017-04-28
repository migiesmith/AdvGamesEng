using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class ObjectiveItem : MonoBehaviour
    {
        NVRInteractableItem missionInt;
        AirlockControl airlock;

        // Use this for initialization
        void Start()
        {
            missionInt = GetComponent<NVRInteractableItem>();
            airlock = FindObjectOfType<AirlockControl>();

            missionInt.OnBeginInteraction.AddListener(airlock.openAirlock);
            missionInt.OnEndInteraction.AddListener(airlock.closeAirlock);
            missionInt.OnUseButtonDown.AddListener(addToInventory);
        }

        void addToInventory()
        {
            if (FindObjectOfType<LootInventory>().addLoot(gameObject))
            {
                missionInt.OnEndInteraction.RemoveListener(airlock.closeAirlock);
                Destroy(gameObject);
            }
            //airlock.openAirlock();
        }
    }
}
