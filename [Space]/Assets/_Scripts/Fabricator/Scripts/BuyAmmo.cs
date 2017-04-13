using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAmmo : MonoBehaviour
{
    private Shop2 shop;
    private DoorSlider slider;

    // Use this for initialization
    void Start()
    {
        shop = transform.root.GetComponentInChildren<Shop2>();
        slider = GetComponent<DoorSlider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            shop.buyAmmo();
            slider.open();
        }
        if (slider.getState() == DoorSlider.DoorState.OPEN)
            slider.close();
    }
    void OnTriggerEnter(Collider other)
    {
        if (slider.getState() == DoorSlider.DoorState.CLOSED && other.transform.parent.name.Contains("Hand"))
        {
            shop.buyAmmo();
            slider.open();
        }
    }
}
