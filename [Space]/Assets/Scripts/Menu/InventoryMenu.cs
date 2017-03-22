using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class InventoryMenu : MonoBehaviour {
    
    AudioSource buttonClick;
    LootInventory lootInv;

    int inventoryIndex = 0;

    // Use this for initialization
    void Start () {
        lootInv = FindObjectOfType<LootInventory>();
        buttonClick = this.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}


    public void previousInventory()
    {
        
    }


    public void nextInventory()
    {
        
    }


    public void changeInventory()
    {
        
    }
}
