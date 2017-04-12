using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    public List<GameObject> inCombat = new List<GameObject>();
    // Use this for initialization

    public void newThreat(GameObject threat)
    {
        inCombat.Add(threat);
    }

    public void threatOver(GameObject threat)
    {
        inCombat.Remove(threat);
    }

    public bool isInCombat()
    {
        if (inCombat.Count > 0)
            return true;
        else
            return false;
    }
}
