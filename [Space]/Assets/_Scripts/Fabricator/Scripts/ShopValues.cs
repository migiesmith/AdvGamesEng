using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopValues : MonoBehaviour {

    //public int price;
    //public string name;
    public string description;
	
    public Material image;

	public int metals;
	public int organics;
	public int fuel;
	//public float radioactive;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void buy()
    {
        Numbers.metals -= metals;
        Numbers.organics -= organics;
        Numbers.fuel -= fuel;
    }
    public void sell()
    {
        Numbers.metals += (int)(metals*0.8f);
        Numbers.organics += (int)(organics*0.8);
        Numbers.fuel += (int)(fuel * 0.8);
        Destroy(this.gameObject);
    }
}
