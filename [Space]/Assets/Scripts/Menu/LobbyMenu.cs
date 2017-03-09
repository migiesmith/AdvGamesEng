using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu : MonoBehaviour {

    GameObject lobbyUI = null;
    GameObject creditsUI = null;
    GameObject settingsUI = null;
    CanvasGroup lobbyCG;
    CanvasGroup creditsCG;
    CanvasGroup settingsCG;
    bool creditsRunning = false;

    Animation anim;
    GameObject player;

    Persistence game;
    Settings settings;
    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    // Use this for initialization
    void Start () {

        lobbyUI = GameObject.Find("LobbyMenu");
        creditsUI = GameObject.Find("Credits");
        settingsUI = GameObject.Find("Settings");
        player = GameObject.Find("MenuPlayer");

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
        if (Input.GetKeyDown("c"))
        {
            runCredits();
        }

        device = SteamVR_Controller.Input((int)trackedObject.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Trigger();
        }

        if (creditsRunning && !anim.isPlaying)
        {
            stopCredits();
        }
    }


   public void Trigger()
    {
        Debug.Log("HERE");
        RaycastHit seen;
        Ray direction = new Ray(player.transform.position, player.transform.forward);
        if (Physics.Raycast(direction, out seen, 5.0f))
        {
            if (seen.collider.tag == "SaveButton") //in the editor, tag anything you want to interact with and use it here
            {
                saveGame();
            }
            else if (seen.collider.tag == "ExitButton")
            {
                exitGame();
            }
            else if (seen.collider.tag == "SettingsButton")
            {
                displaySettings();
            }
            else if (seen.collider.tag == "CreditsButton")
            {
                runCredits();
            }

        }
        Debug.DrawRay(transform.position, transform.forward, Color.black, 1);
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
    }


    public void saveGame()
    {
        Debug.Log("Saving Game");
        game.saveGame();
    }
}
