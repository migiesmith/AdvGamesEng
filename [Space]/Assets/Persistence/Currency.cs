using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour {

    int metalAmount;
    int organicAmount;
    int fuelAmount;
    int radioactiveAmount;

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
}
