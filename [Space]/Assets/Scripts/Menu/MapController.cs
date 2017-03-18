/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapController : MonoBehaviour {

    [System.Serializable]
    public class TriggerHandler : UnityEvent{}
    public TriggerHandler onEnter;
    public TriggerHandler onExit;

	public GameObject skyBox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider){
		if(collider.tag == "PlayerSensor")
			onEnter.Invoke();
	}

	void OnTriggerExit(Collider collider){
		if(collider.tag == "PlayerSensor")
			onExit.Invoke();
	}

}
