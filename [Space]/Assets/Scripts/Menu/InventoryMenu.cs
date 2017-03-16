using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class InventoryMenu : MonoBehaviour {

    GameObject inventory;
    
    AudioSource buttonClick;
    NVRPlayer player;

    // Use this for initialization
    void Start () {
        inventory = GameObject.Find("InventoryObject");
        player = GameObject.FindObjectOfType<NVRPlayer>();
        buttonClick = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        inventory.transform.position = player.LeftHand.transform.position;
        inventory.transform.rotation = player.LeftHand.transform.rotation;
	}

    public void changeList()
    {

    }
}
