using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCost : ResourceDisplay
{
    public new void updateCost(List<int> values)
    {
        List<int> playerVals = FindObjectOfType<Currency>().getCurrency();

        string output = "\t\t\t\t\n";

        for (int index = 0; index < values.Count; ++index)
        {
            if (values[index] > playerVals[index])
                output += "<color=#ff8181ff>" + values[index] + "</color>\t";
            else
                output += values[index] + "\t";
        }
        display.text = output;
    }
}
