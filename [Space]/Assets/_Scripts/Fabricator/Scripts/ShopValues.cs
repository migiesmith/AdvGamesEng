using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopValues : MonoBehaviour
{
    public string description;

    public Material image;

    public int metals;
    public int organics;
    public int fuel;
    public int radioactive;

    public void buy()
    {
        FindObjectOfType<Currency>().substractCurrency(metals, organics, fuel, radioactive);
    }

    public void sell()
    {
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
