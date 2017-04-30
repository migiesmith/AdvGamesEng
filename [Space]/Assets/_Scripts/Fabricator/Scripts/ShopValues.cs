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
	public int radioactive;


	// Use this for initialization
	void Start () {
		
	}
	
    public void buy()
    {
        /*
        Numbers.metals -= metals;
        Numbers.organics -= organics;
        Numbers.fuel -= fuel;
        Numbers.radioactive -= radioactive;
        */
        FindObjectOfType<Currency>().substractCurrency(metals, organics, fuel, radioactive);
    }
    public void sell()
    {
        /*
        Numbers.metals += (int)(metals*0.8f);
        Numbers.organics += (int)(organics*0.8f);
        Numbers.fuel += (int)(fuel * 0.8f);
        Numbers.radioactive += (int)(radioactive * 0.8f);
        */
        FindObjectOfType<Currency>().addCurrency(metals, organics, fuel, radioactive);
    }
    public List<int> getVals()
    {
        List<int> vals = new List<int>();
        vals.Add(metals);
        vals.Add(organics);
        vals.Add(fuel);
        vals.Add(radioactive);
        return vals;
    }
}
