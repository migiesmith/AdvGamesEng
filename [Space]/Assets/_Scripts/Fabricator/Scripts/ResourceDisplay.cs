using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour {

    public TextMesh display;
    
    public void updateCost(List<int> values)
    {
        string output = "\t\t\t\t\n";

        foreach(int value in values)
        {
            output += value + "\t";
        }
        display.text = output;
    }
}
