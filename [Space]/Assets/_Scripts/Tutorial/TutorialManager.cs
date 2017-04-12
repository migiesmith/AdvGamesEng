using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class TutorialManager : MonoBehaviour {

    NVRPlayer player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<NVRPlayer>();
    }
	
	// Update is called once per frame
	void Update () {
        //The lobby ship at the tutorial end is the only section this far down on the x-axis.
		if(player.transform.position.x < -16)
        {
            finishTutorial();
        }
        enemyPatrol();
	}

    void finishTutorial()
    {
        GameObject.Find("Persistence").GetComponent<Persistence>().tutorialDone = true;
        this.GetComponent<SteamVR_LoadLevel>().Trigger();
    }


    void enemyPatrol()
    {

    }
}
