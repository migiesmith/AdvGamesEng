using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

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
        DoorSlider.DoorState doorState = doorSlider.getState();
        if(doorState == DoorSlider.DoorState.OPEN || doorState == DoorSlider.DoorState.OPENING)
            return;

        if(other.tag == "PlayerSensor" || other.tag == "EnemySensor"){
            doorSlider.open();
        }
    }

    void tryCloseDoor(Collider other){
        DoorSlider.DoorState doorState = doorSlider.getState();
        if(doorState == DoorSlider.DoorState.CLOSED || doorState == DoorSlider.DoorState.CLOSING)
            return;

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
