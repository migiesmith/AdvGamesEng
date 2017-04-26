using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewtonVR;

public class ShipTutorialManager : MonoBehaviour {

    public enum ShipTutorial
    {
        FABRICATOR_LOCATION,
        FABRICATOR_SELL,
        FABRICATOR_BROWSE,
        FABRICATOR_END,
        ARMORY_LOCATION,
        ARMORY_USAGE,
        MAP_LOCATION,
        MAP_ENTER,
        MAP_PICKUP
    }

    private ShipTutorial stage;

    public Text FabricatorText;
    public Text ArmoryText;
    public Text MapText;

    public GameObject[] waypoints;
    public TextMesh playerWaypoint;
    public Collider playerDetector;

	// Use this for initialization
	void Start () {
        playerWaypoint.text = "";
        stage = ShipTutorial.FABRICATOR_LOCATION;
        transform.position = waypoints[0].transform.position;
        ArmoryText.transform.parent.gameObject.SetActive(false);
        MapText.transform.parent.gameObject.SetActive(false);
	}
	
    public void itemsSold()
    {
        if (stage == ShipTutorial.FABRICATOR_SELL)
        {
            FabricatorText.text = "The Resource cost of the selected item is displayed beneath the \"Create\" button.\n\nUse the arrow buttons to the left of the screen to browse the available items.";
            stage = ShipTutorial.FABRICATOR_BROWSE;
        }
    }

    public void itemsBrowsed()
    {
        if (stage == ShipTutorial.FABRICATOR_BROWSE)
        {
            FabricatorText.text = "Unwanted equipment can be recycled by dropping it in the box to the left of the screen, returning 80% of its Resource cost.\n\nTurn around and proceed to the next waypoint.";
            transform.position = waypoints[1].transform.position;
            playerWaypoint.text = "";
            playerDetector.enabled = true;
            ArmoryText.transform.parent.gameObject.SetActive(true);
            stage = ShipTutorial.ARMORY_LOCATION;
        }
    }

    public void weaponWithdrawn()
    {
        if (stage == ShipTutorial.ARMORY_USAGE)
        {
            ArmoryText.text = "Turn around and proceed to the next waypoint.";
            transform.position = waypoints[2].transform.position;
            playerWaypoint.text = "";
            playerDetector.enabled = true;
            MapText.transform.parent.gameObject.SetActive(true);
            stage = ShipTutorial.MAP_LOCATION;
        }
    }

    public void tutorialComplete()
    {
        if (stage == ShipTutorial.MAP_PICKUP)
        {
            MapText.text = "Tutorial complete.";
            FindObjectOfType<Persistence>().tutorialDone = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<NVRPlayer>() != null)
        {
            if (stage == ShipTutorial.FABRICATOR_LOCATION)
            {
                playerWaypoint.text = "";
                playerDetector.enabled = false;
                stage = ShipTutorial.FABRICATOR_SELL;
            }
            else if (stage == ShipTutorial.ARMORY_LOCATION)
            {
                playerWaypoint.text = "";
                playerDetector.enabled = false;
                FabricatorText.transform.parent.gameObject.SetActive(false);
                stage = ShipTutorial.ARMORY_USAGE;
            }
            else if (stage == ShipTutorial.MAP_LOCATION)
            {
                transform.position = waypoints[3].transform.position;
                ArmoryText.transform.parent.gameObject.SetActive(false);
                stage = ShipTutorial.MAP_ENTER;
            }
            else if (stage == ShipTutorial.MAP_ENTER)
            {
                playerWaypoint.text = "";
                playerDetector.enabled = false;
                stage = ShipTutorial.MAP_PICKUP;
            }
        }
    }
}
