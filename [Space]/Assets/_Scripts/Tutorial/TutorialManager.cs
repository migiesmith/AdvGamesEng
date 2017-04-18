using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class TutorialManager : MonoBehaviour {

    public enum TutorialStage
    {
        TELEPORT,
        DASH,
        MELEE_LOCATION,
        MELEE_PICKUP,
        MELEE_COMBAT,
        RANGE_LOCATION,
        RANGE_PISTOL_PICKUP,
        RANGE_AMMO_PICKUP,
        RANGE_RELOAD,
        RANGE_COMBAT,
        ENEMY_LOCATION,
        ENEMY_COMBAT,
        LOOT_LOCATION,
        LOOT_PICKUP,
        SHOP,
        OBJECTIVE,
        DONE
    }

    public TutorialStage stage = TutorialStage.OBJECTIVE;

    NVRPlayer player;
    GameObject objective;
    GameObject animatedDoor;
    Quaternion rot;

    space.Dash2 dash;
    public GameObject playerWaypoint;
    public List<Transform> waypoints;
    public List<GameObject> meleeWaypoints;
    public List<GameObject> rangeWaypoints;
    public GameObject botWaypoint;

    private int meleeTargets = 2;
    public int rangeTargets = 3;

    public bool botDead = false;

    public bool lootSold = false;

    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<NVRPlayer>();
        dash = player.GetComponentInChildren<space.Dash2>();
        playerWaypoint.transform.position = waypoints[0].position;
        dash.enabled = false;
        objective = GameObject.Find("BlackBox");
        animatedDoor = GameObject.Find("AnimatedDoor");
        rot = animatedDoor.transform.rotation;

        Debug.Log(animatedDoor.transform.rotation.w + ", " + animatedDoor.transform.rotation.x + ", " + animatedDoor.transform.rotation.y + ", " + animatedDoor.transform.rotation.z);
        Debug.Log("Range: " + GameObject.Find("RangeDoor").transform.rotation.w + ", " + GameObject.Find("RangeDoor").transform.rotation.x + ", " + GameObject.Find("RangeDoor").transform.rotation.y + ", " + GameObject.Find("RangeDoor").transform.rotation.z);
    }
	
	// Update is called once per frame
	void Update () {

        switch (stage)
        {
            case TutorialStage.TELEPORT:
                {
                    //Infront of the TeleportDoor
                    if(player.transform.position.z > -14.0f)
                    {
                        dash.enabled = true;
                        playerWaypoint.transform.position = waypoints[1].position;
                        stage = TutorialStage.DASH;                       
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();                     
                    }
                }
                break;
            case TutorialStage.DASH:
                {
                    //Infront of the DashDoor
                    if (player.transform.position.z > 0.0f)
                    {
                        playerWaypoint.transform.position = waypoints[2].position;
                        stage = TutorialStage.MELEE_LOCATION;
                        GameObject.Find("DashDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(1.4f, 1.95f, 3.6f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.MELEE_LOCATION:
                {
                    if (player.transform.position.z < 12.0f && player.transform.position.x < -4.0f)
                    {
                        playerWaypoint.transform.position = GameObject.Find("Pipe").transform.position + 0.8f * Vector3.up;
                        stage = TutorialStage.MELEE_PICKUP;
                    }
                }
                break;
            case TutorialStage.MELEE_COMBAT:
                {
                    //If all Melee Targets have been destroyed.
                    if(meleeTargets <= 0){
                        playerWaypoint.SetActive(true);
                        playerWaypoint.transform.position = waypoints[3].position;
                        stage = TutorialStage.RANGE_LOCATION;
                        GameObject.Find("RangeDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(3.88f, 1.95f, 8.61f);
                        animatedDoor.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.RANGE_LOCATION:
                {
                    if (player.transform.position.z < 12.0f && player.transform.position.x > 4.0f)
                    {
                        playerWaypoint.transform.position = GameObject.Find("LP").transform.position + 0.8f * Vector3.up;
                        stage = TutorialStage.RANGE_PISTOL_PICKUP;
                    }
                }
                break;
            case TutorialStage.RANGE_COMBAT:
                {
                    //If all Range Targets have been destroyed.
                    if (rangeTargets <= 0)
                    {
                        playerWaypoint.SetActive(true);
                        playerWaypoint.transform.position = waypoints[4].position;
                        stage = TutorialStage.ENEMY_LOCATION;
                        GameObject.Find("WeaponCanvas").gameObject.SetActive(false);
                        GameObject.Find("WeaponDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(1.4f, 1.95f, 13.0f);
                        animatedDoor.transform.rotation = rot;
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.ENEMY_LOCATION:
                {
                    if (player.transform.position.z > 12.0f)
                    {
                        playerWaypoint.SetActive(false);
                        botWaypoint.SetActive(true);
                        stage = TutorialStage.ENEMY_COMBAT;
                    }
                }
                break;
            case TutorialStage.ENEMY_COMBAT:
                {
                    //Check to see if the enemy has been killed.
                    if (botDead)
                    {
                        playerWaypoint.SetActive(true);
                        playerWaypoint.transform.position = waypoints[5].position;
                        stage = TutorialStage.LOOT_LOCATION;
                        GameObject.Find("InstructionCanvas").gameObject.SetActive(false);
                        GameObject.Find("EnemyDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(1.4f, 1.95f, 27.27f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.LOOT_LOCATION:
                {
                    if (player.transform.position.z > 30.0f)
                    {
                        playerWaypoint.transform.position = GameObject.Find("Core").transform.position + 0.8f*Vector3.up;
                        stage = TutorialStage.SHOP;
                    }
                }
                break;
            case TutorialStage.SHOP:
                {
                    //Check to see if the Powercell item has been sold.
                    if (lootSold)
                    {
                        stage = TutorialStage.OBJECTIVE;
                        GameObject.Find("ObjectiveDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(-5.81f, 1.95f, 36.93f);
                        animatedDoor.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.OBJECTIVE:
                {
                    //Check to see if the BlackBox has been picked up and moved.
                    Debug.Log("Position: " + objective.transform.position.x + ", " + objective.transform.position.z);
                    if ((objective.transform.position.x > -11.0f) || (objective.transform.position.x < -12.0f) || (objective.transform.position.z > 34.1f))
                    {
                        stage = TutorialStage.DONE;
                        GameObject.Find("LobbyDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(-15.12f, 1.95f, 36.93f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.DONE:
                {
                    //The lobby ship at the tutorial end is the only section this far down on the x-axis.
                    if (player.transform.position.x < -16)
                    {
                        finishTutorial();
                    }
                }
                break;
        }     
	}

    void finishTutorial()
    {
        GameObject.Find("Persistence").GetComponent<Persistence>().tutorialDone = true;
        this.GetComponent<SteamVR_LoadLevel>().Trigger();
    }

    public void pipePickup()
    {
        if (stage == TutorialStage.MELEE_PICKUP)
        {
            playerWaypoint.SetActive(false);
            foreach (GameObject meleeWaypoint in meleeWaypoints)
            {
                meleeWaypoint.transform.parent.gameObject.SetActive(true);
            }
            stage = TutorialStage.MELEE_COMBAT;
        }
    }

    public void meleeKill()
    {
        --meleeTargets;
    }

    public void pistolPickup()
    {
        if (stage == TutorialStage.RANGE_PISTOL_PICKUP)
        {
            playerWaypoint.transform.position = GameObject.Find("LP_Magazine").transform.position + new Vector3(0.0f, 0.8f, 0.0f);
            stage = TutorialStage.RANGE_AMMO_PICKUP;
        }
    }

    public void ammoPickup()
    {
        if (stage == TutorialStage.RANGE_AMMO_PICKUP)
        {
            playerWaypoint.SetActive(false);
            stage = TutorialStage.RANGE_RELOAD;
        }
    }

    public void pistolReload()
    {
        if (stage == TutorialStage.RANGE_RELOAD)
        {
            foreach (GameObject rangeWaypoint in rangeWaypoints)
            {
                rangeWaypoint.transform.parent.gameObject.SetActive(true);
            }
            stage = TutorialStage.RANGE_COMBAT;
        }
    }

    public void rangeKill()
    {
        --rangeTargets;
    }

    public void botKill()
    {
        botDead = true;
    }

    public void lootPickup()
    {
        if (stage == TutorialStage.LOOT_PICKUP)
        {
            playerWaypoint.transform.position = waypoints[5].position;
            stage = TutorialStage.SHOP;
        }
    }
}
