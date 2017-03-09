using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class LobbyMenu : MonoBehaviour {

    GameObject lobbyUI = null;
    GameObject creditsUI = null;
    GameObject settingsUI = null;
    CanvasGroup lobbyCG;
    CanvasGroup creditsCG;
    CanvasGroup settingsCG;
    bool creditsRunning = false;

    Animation anim;
    NVRPlayer player;

    Persistence game;
    Settings settings;
    SteamVR_TrackedObject trackedObject;

    // Use this for initialization
    void Start () {

        lobbyUI = GameObject.Find("LobbyMenu");
        creditsUI = GameObject.Find("Credits");
        settingsUI = GameObject.Find("Settings");
        player = GameObject.FindObjectOfType<NVRPlayer>();

        lobbyCG = lobbyUI.GetComponent<CanvasGroup>();
        creditsCG = creditsUI.GetComponent<CanvasGroup>();
        settingsCG = settingsUI.GetComponent<CanvasGroup>();

        trackedObject = GetComponent<SteamVR_TrackedObject>();

        anim = creditsUI.GetComponent<Animation>();

        lobbyCG.alpha = 1.0f;
        lobbyCG.interactable = true;
        lobbyCG.blocksRaycasts = true;

        creditsCG.alpha = 0.0f;
        creditsCG.interactable = false;
        creditsCG.blocksRaycasts = false;

        settingsCG.alpha = 0.0f;
        settingsCG.interactable = false;
        settingsCG.blocksRaycasts = false;

    }
	
	// Update is called once per frame
	void Update () {
       /* if (Input.GetKeyDown("c"))
        {
            runCredits();
        }*/
        

        if (creditsRunning && !anim.isPlaying)
        {
            stopCredits();
        }
    }


    public void runCredits()
    {
        lobbyCG.alpha = 0.0f;
        lobbyCG.interactable = false;
        lobbyCG.blocksRaycasts = false;

        creditsCG.alpha = 1.0f;
        creditsCG.interactable = true;
        creditsCG.blocksRaycasts = true;

        anim.Play();

        creditsRunning = true;
    }


    public void stopCredits()
    {
        lobbyCG.alpha = 1.0f;
        lobbyCG.interactable = true;
        lobbyCG.blocksRaycasts = true;

        creditsCG.alpha = 0.0f;
        creditsCG.interactable = false;
        creditsCG.blocksRaycasts = false;

        anim.Stop();

        creditsRunning = false;
    }


    public void displaySettings()
    {
        lobbyCG.alpha = 0.0f;
        lobbyCG.interactable = false;
        lobbyCG.blocksRaycasts = false;

        settingsCG.alpha = 1.0f;
        settingsCG.interactable = true;
        settingsCG.blocksRaycasts = true;
    }


    public void hideSettings()
    {
        lobbyCG.alpha = 1.0f;
        lobbyCG.interactable = true;
        lobbyCG.blocksRaycasts = true;

        settingsCG.alpha = 0.0f;
        settingsCG.interactable = false;
        settingsCG.blocksRaycasts = false;
    }

    public void exitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
        Debug.Break();
    }


    public void saveGame()
    {
        Debug.Log("Saving Game");
        game.saveGame();
    }
}
