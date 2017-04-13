using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class TutorialManager : MonoBehaviour {

    public enum TutorialStage
    {
        TELEPORT,
        DASH,
        MELEE,
        RANGE,
        ENEMY,
        SHOP,
        OBJECTIVE,
        DONE
    }

    public TutorialStage stage = TutorialStage.OBJECTIVE;

    NVRPlayer player;
    GameObject objective;
    GameObject animatedDoor;
    Quaternion rot;

    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<NVRPlayer>();
        objective = GameObject.Find("BlackBox");
        animatedDoor = GameObject.Find("AnimatedDoor");
        rot = animatedDoor.transform.rotation;

        Debug.Log(animatedDoor.transform.rotation.w + ", " + animatedDoor.transform.rotation.x + ", " + animatedDoor.transform.rotation.y + ", " + animatedDoor.transform.rotation.z);
        Debug.Log("Range: " + GameObject.Find("RangeDoor").transform.rotation.w + ", " + GameObject.Find("RangeDoor").transform.rotation.x + ", " + GameObject.Find("RangeDoor").transform.rotation.y + ", " + GameObject.Find("RangeDoor").transform.rotation.z);
    }
	
	// Update is called once per frame
	void Update () {

        switch (stage)
        {
            case TutorialStage.TELEPORT:
                {
                    //Infront of the TeleportDoor
                    if(player.transform.position.z > -14.0f)
                    {
                        stage = TutorialStage.DASH;                       
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();                     
                    }
                }
                break;
            case TutorialStage.DASH:
                {
                    //Infront of the DashDoor
                    if (player.transform.position.z > 0.0f)
                    {
                        stage = TutorialStage.MELEE;
                        GameObject.Find("DashDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(1.4f, 1.95f, 3.6f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.MELEE:
                {
                    //If all Melee Targets have been destroyed.
                    if(GameObject.Find("MeleeTarget") == null){
                        stage = TutorialStage.RANGE;
                        GameObject.Find("RangeDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(3.88f, 1.95f, 8.61f);
                        animatedDoor.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.RANGE:
                {
                    //If all Range Targets have been destroyed.
                    if (GameObject.Find("RangeTarget") == null)
                    {
                        stage = TutorialStage.ENEMY;
                        GameObject.Find("WeaponCanvas").gameObject.SetActive(false);
                        GameObject.Find("WeaponDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(1.4f, 1.95f, 13.0f);
                        animatedDoor.transform.rotation = rot;
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.ENEMY:
                {
                    //Check to see if the enemy has been killed.
                    if (GameObject.Find("bot") == null)
                    {
                        stage = TutorialStage.SHOP;
                        GameObject.Find("InstructionCanvas").gameObject.SetActive(false);
                        GameObject.Find("EnemyDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(1.4f, 1.95f, 27.27f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.SHOP:
                {
                    //Check to see if the Powercell item has been sold.
                    if (GameObject.Find("PowerCell") == null)
                    {
                        stage = TutorialStage.OBJECTIVE;
                        GameObject.Find("ObjectiveDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(-5.81f, 1.95f, 36.93f);
                        animatedDoor.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.OBJECTIVE:
                {
                    //Check to see if the BlackBox has been picked up and moved.
                    Debug.Log("Position: " + objective.transform.position.x + ", " + objective.transform.position.z);
                    if ((objective.transform.position.x > -11.0f) || (objective.transform.position.x < -12.0f) || (objective.transform.position.z > 34.1f))
                    {
                        stage = TutorialStage.DONE;
                        GameObject.Find("LobbyDoor").gameObject.SetActive(false);
                        animatedDoor.transform.position = new Vector3(-15.12f, 1.95f, 36.93f);
                        animatedDoor.GetComponentInChildren<TutorialDoor>().runAnimation();
                    }
                }
                break;
            case TutorialStage.DONE:
                {
                    //The lobby ship at the tutorial end is the only section this far down on the x-axis.
                    if (player.transform.position.x < -16)
                    {
                        finishTutorial();
                    }
                }
                break;
        }     
	}

    void finishTutorial()
    {
        GameObject.Find("Persistence").GetComponent<Persistence>().tutorialDone = true;
        this.GetComponent<SteamVR_LoadLevel>().Trigger();
    }
}
