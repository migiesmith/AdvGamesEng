using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (DoorSlider))]
public class UpScript : MonoBehaviour
{

    private Shop2 shop;
    private DoorSlider slider;

	// Use this for initialization
	void Start ()
    {
        shop = transform.root.GetComponentInChildren<Shop2>();
        slider = GetComponent<DoorSlider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            shop.up();
            slider.open();
        }
        if(slider.getState() == DoorSlider.DoorState.OPEN)
            slider.close();
    }
    void OnTriggerEnter(Collider other)
    {
        shop.up();
        slider.open();
    }
}
