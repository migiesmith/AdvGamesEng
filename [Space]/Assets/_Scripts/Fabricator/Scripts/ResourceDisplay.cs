using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour {

    protected TextMesh display;
	// Use this for initialization
	void Start () {
        display = GetComponent<TextMesh>();
        display.text = "\t\t\t\t";
    }

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
