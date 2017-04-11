using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButtonScript : MonoBehaviour {

    private Shop2 shop;
    private DoorSlider slider;

	// Use this for initialization
	void Start () {
        shop = transform.root.GetComponentInChildren<Shop2>();	
        slider = GetComponent<DoorSlider>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            shop.spawn();
            slider.open();
        }
        if(slider.getState() == DoorSlider.DoorState.OPEN)
            slider.close();
    }
    void OnTriggerEnter(Collider other)
    {
        if (slider.getState() == DoorSlider.DoorState.CLOSED)
        {
            shop.spawn();
            slider.open();
        }
    }
}
