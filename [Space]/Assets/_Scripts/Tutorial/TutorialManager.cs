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

    public GameObject animatedDoor;
    private TutorialDoor doorControl;
    public GameObject[] doors;
    public GameObject[] canvases;

    private int meleeTargets;
    private int rangeTargets;

    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<NVRPlayer>();
        dash = player.GetComponentInChildren<space.Dash2>();
        dash.enabled = false;
        transform.position = waypoints[0].position;
        meleeTargets = 2;//meleeWaypoints.Length;
        rangeTargets = 3;//rangeWaypoints.Length;
        doorControl = animatedDoor.GetComponentInChildren<TutorialDoor>();
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
        if (stage == TutorialStage.MELEE_PICKUP)
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
            doors[1].SetActive(false);
            animatedDoor.transform.position = doors[1].transform.position;
            animatedDoor.transform.rotation = doors[1].transform.rotation;
            doorControl.runAnimation();
            transform.position = waypoints[3].position;
            enableWaypoint();
            stage = TutorialStage.RANGE_LOCATION;
        }
        Debug.Log(meleeTargets + " melee targets remaining");
    }

    public void pistolPickup()
    {
        if (stage == TutorialStage.RANGE_PISTOL_PICKUP)
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
        if (stage == TutorialStage.RANGE_RELOAD)
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
            doors[2].SetActive(false);
            animatedDoor.transform.position = doors[2].transform.position;
            animatedDoor.transform.rotation = doors[2].transform.rotation;
            doorControl.runAnimation();
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
        doors[3].SetActive(false);
        animatedDoor.transform.position = doors[3].transform.position;
        doorControl.runAnimation();
    }

    public void lootPickup()
    {
        if (stage == TutorialStage.LOOT_PICKUP)
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
            doors[4].SetActive(false);
            animatedDoor.transform.position = doors[4].transform.position;
            animatedDoor.transform.rotation = doors[4].transform.rotation;
            doorControl.runAnimation();
            stage = TutorialStage.OBJECTIVE_LOCATION;
        }
    }

    public void objectivePickup()
    {
        if (stage == TutorialStage.OBJECTIVE_PICKUP)
        {
            transform.position = waypoints[7].transform.position;
            stage = TutorialStage.OBJECTIVE_AIRLOCK;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<NVRPlayer>() != null)
        {
            if (stage == TutorialStage.TELEPORT)
            {
                doorControl.runAnimation();
                transform.position = waypoints[1].position;
                dash.enabled = true;
                stage = TutorialStage.DASH;
            }
            else if (stage == TutorialStage.DASH)
            {
                doors[0].SetActive(false);
                animatedDoor.transform.position = doors[0].transform.position;
                doorControl.runAnimation();
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
