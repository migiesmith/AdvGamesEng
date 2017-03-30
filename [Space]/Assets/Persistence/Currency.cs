using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour {

    int metalAmount = 0;
    int organicAmount = 0;
    int fuelAmount = 0;
    int radioactiveAmount = 0;

    public void addCurrency(int metal, int organic, int fuel, int radioactive)
    {
        metalAmount += metal;
        organicAmount += organic;
        fuelAmount += fuel;
        radioactiveAmount += radioactive;
    }

    public void substractCurrency(int metal, int organic, int fuel, int radioactive)
    {
        metalAmount -= metal;
        organicAmount -= organic;
        fuelAmount -= fuel;
        radioactiveAmount -= radioactive;
    }

    public void setCurrency(List<int> currency)
    {
        metalAmount = currency[0];
        organicAmount = currency[1];
        fuelAmount = currency[2];
        radioactiveAmount = currency[3];
    }

    public List<int> getCurrency()
    {
        List<int> send = new List<int>();
        send.Add(metalAmount);
        send.Add(organicAmount);
        send.Add(fuelAmount);
        send.Add(radioactiveAmount);
        return send;
    }

    //public void setNumbers()
    //{
    //    Numbers.fuel = fuelAmount;
    //    Numbers.metals = metalAmount;
    //    Numbers.organics = organicAmount;
    //    Numbers.radioactive = radioactiveAmount;
    //}
}
