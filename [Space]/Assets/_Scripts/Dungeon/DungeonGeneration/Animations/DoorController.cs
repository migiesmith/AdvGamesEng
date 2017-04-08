/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    // Script used to slide the door
    public DoorSlider doorSlider;


	// Use this for initialization
	void Start () {
        if(this.doorSlider == null){
            Debug.Log("No DoorSlider attatched to this DoorController.");
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void tryOpenDoor(Collider other){
        // Check the current door state
        DoorSlider.DoorState doorState = doorSlider.getState();
        // Don't open if already open or being opened
        if(doorState == DoorSlider.DoorState.OPEN || doorState == DoorSlider.DoorState.OPENING)
            return;
        
        // Check if the door can be opened by the collider tag
        if(other.tag == "PlayerSensor" || other.tag == "EnemySensor"){
            doorSlider.open();
        }
    }

    void tryCloseDoor(Collider other){
        // Check the current door state
        DoorSlider.DoorState doorState = doorSlider.getState();
        // Don't open if already closed or being closed
        if(doorState == DoorSlider.DoorState.CLOSED || doorState == DoorSlider.DoorState.CLOSING)
            return;

        // Check if the door can be closed by the collider tag
        if(other.tag == "PlayerSensor" || other.tag == "EnemySensor"){            
            doorSlider.close();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        tryOpenDoor(other);
    }    

    private void OnTriggerStay(Collider other)
    {
        tryOpenDoor(other);
    }    

    private void OnTriggerExit(Collider other)
    {
        tryCloseDoor(other);
    }
}
