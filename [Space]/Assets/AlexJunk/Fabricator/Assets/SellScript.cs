using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellScript : MonoBehaviour {

    private DoorSlider slider;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<DoorSlider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            FindObjectOfType<LootInventory>().sellAll() ;
            slider.open();
        }
        if (slider.getState() == DoorSlider.DoorState.OPEN)
            slider.close();
    }
    void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<LootInventory>().sellAll();
        slider.open();
    }
}
