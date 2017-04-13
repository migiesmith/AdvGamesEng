using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (DoorSlider))]
public class DownScript : MonoBehaviour {

    private Shop2 shop;
    private DoorSlider slider;

	// Use this for initialization
	void Start () {
        shop = transform.root.GetComponentInChildren<Shop2>();		
        slider = GetComponent<DoorSlider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("w"))
        {
            shop.down();
            slider.open();
        }
        if(slider.getState() == DoorSlider.DoorState.OPEN)
            slider.close();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.name.Contains("Hand"))
        {
            shop.down();
            slider.open();
        }
    }
}
