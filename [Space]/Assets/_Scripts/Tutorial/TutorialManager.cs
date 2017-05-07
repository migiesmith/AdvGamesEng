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
        LOOT_INVENTORY,
        OBJECTIVE_LOCATION,
        OBJECTIVE_PICKUP,
        OBJECTIVE_AIRLOCK
    }

    public TutorialStage stage = TutorialStage.TELEPORT;

    NVRPlayer player;
    Quaternion rot;

    space.Dash2 dash;
    public TextMesh playerWaypoint;
    public Collider playerDetector;
    public Transform[] waypoints;
    public GameObject[] objectWaypoints;
    public GameObject[] meleeWaypoints;
    public GameObject[] rangeWaypoints;
    public GameObject botWaypoint;

    public DoorSlider[] doors;
    public GameObject[] canvases;

    private int meleeTargets;
    private int rangeTargets;

    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<NVRPlayer>();
        dash = player.GetComponentInChildren<space.Dash2>();
        dash.enabled = false;
        transform.position = waypoints[0].position;
        meleeTargets = meleeWaypoints.Length;
        rangeTargets = rangeWaypoints.Length;
    }

    void Update()
    {
        if (Input.GetKeyDown("s")) //Used to skip the tutorial //Used for debugging purposes.
        {
            objectivePickup();
        }
    }
	
    void enableWaypoint()
    {
        playerWaypoint.text = "";
        playerDetector.enabled = true;
    }

    void disableWaypoint()
    {
        playerWaypoint.text = "";
        playerDetector.enabled = false;
    }

    public void pipePickup()
    {
        if (stage == TutorialStage.MELEE_LOCATION || stage == TutorialStage.MELEE_PICKUP)
        {
            disableWaypoint();
            foreach (GameObject meleeWaypoint in meleeWaypoints)
                meleeWaypoint.transform.parent.gameObject.SetActive(true);
            stage = TutorialStage.MELEE_COMBAT;
        }
    }

    public void meleeKill()
    {
        --meleeTargets;
        if (meleeTargets <= 0)
        {
            doors[2].open();
            transform.position = waypoints[3].position;
            enableWaypoint();
            stage = TutorialStage.RANGE_LOCATION;
        }
        Debug.Log(meleeTargets + " melee targets remaining");
    }

    public void pistolPickup()
    {
        if (stage == TutorialStage.RANGE_LOCATION || stage == TutorialStage.RANGE_PISTOL_PICKUP)
        {
            transform.position = objectWaypoints[2].transform.position + new Vector3(0.0f, 0.8f, 0.0f);
            stage = TutorialStage.RANGE_AMMO_PICKUP;
        }
    }

    public void ammoPickup()
    {
        if (stage == TutorialStage.RANGE_AMMO_PICKUP)
        {
            disableWaypoint();
            stage = TutorialStage.RANGE_RELOAD;
        }
    }

    public void pistolReload()
    {
        if (stage == TutorialStage.RANGE_AMMO_PICKUP || stage == TutorialStage.RANGE_RELOAD)
        {
            foreach (GameObject rangeWaypoint in rangeWaypoints)
                rangeWaypoint.transform.parent.gameObject.SetActive(true);
            stage = TutorialStage.RANGE_COMBAT;
        }
    }

    public void rangeKill()
    {
        --rangeTargets;
        if (rangeTargets <= 0)
        {
            canvases[5].SetActive(false);
            doors[3].open();
            transform.position = waypoints[4].position;
            enableWaypoint();
            stage = TutorialStage.ENEMY_LOCATION;
        }
        Debug.Log(rangeTargets + " range targets remaining");
    }

    public void botKill()
    {
        transform.position = waypoints[5].position;
        enableWaypoint();
        stage = TutorialStage.LOOT_LOCATION;
        canvases[6].SetActive(false);
        doors[4].open();
    }

    public void lootPickup()
    {
        if (stage == TutorialStage.LOOT_LOCATION || stage == TutorialStage.LOOT_PICKUP)
        {
            disableWaypoint();
            stage = TutorialStage.LOOT_INVENTORY;
        }
    }

    public void lootInventory()
    {
        if (stage == TutorialStage.LOOT_INVENTORY)
        {
            transform.position = waypoints[6].transform.position;
            enableWaypoint();
            doors[5].open();
            stage = TutorialStage.OBJECTIVE_LOCATION;
        }
    }

    public void objectivePickup()
    {
        if (stage == TutorialStage.OBJECTIVE_LOCATION || stage == TutorialStage.OBJECTIVE_PICKUP)
        {
            transform.position = waypoints[7].transform.position;
            stage = TutorialStage.OBJECTIVE_AIRLOCK;
            //tutorial done.
            GameObject.Find("Persistence").GetComponent<Persistence>().tutorialDone = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "DoorSensor")
        {
            if (stage == TutorialStage.TELEPORT)
            {
                doors[0].open();
                transform.position = waypoints[1].position;
                dash.enabled = true;
                stage = TutorialStage.DASH;
            }
            else if (stage == TutorialStage.DASH)
            {
                doors[1].open();
                transform.position = waypoints[2].position;
                stage = TutorialStage.MELEE_LOCATION;
            }
            else if (stage == TutorialStage.MELEE_LOCATION)
            {
                transform.position = objectWaypoints[0].transform.position + 0.8f * Vector3.up;
                stage = TutorialStage.MELEE_PICKUP;
            }
            else if (stage == TutorialStage.RANGE_LOCATION)
            {
                transform.position = objectWaypoints[1].transform.position + 0.8f * Vector3.up;
                stage = TutorialStage.RANGE_PISTOL_PICKUP;
            }
            else if (stage == TutorialStage.ENEMY_LOCATION)
            {
                disableWaypoint();
                botWaypoint.SetActive(true);
                stage = TutorialStage.ENEMY_COMBAT;
            }
            else if (stage == TutorialStage.LOOT_LOCATION)
            {
                transform.position = objectWaypoints[3].transform.position + 0.8f * Vector3.up;
                stage = TutorialStage.LOOT_PICKUP;
            }
            else if (stage == TutorialStage.OBJECTIVE_LOCATION)
            {
                transform.position = objectWaypoints[4].transform.position + 0.8f * Vector3.up;
                stage = TutorialStage.OBJECTIVE_PICKUP;
            }
        }
    }
}
