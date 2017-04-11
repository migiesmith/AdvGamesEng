using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    public int inCombat;
    // Use this for initialization
    void Start() {
        inCombat = 0;
    }

    public void newThreat()
    {
        ++inCombat;
    }

    public void threatOver()
    {
        --inCombat;
    }
}
