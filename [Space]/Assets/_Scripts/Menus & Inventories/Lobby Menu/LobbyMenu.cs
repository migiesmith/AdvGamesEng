using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class LobbyMenu : MonoBehaviour {

    GameObject lobbyUI = null;
    GameObject creditsUI = null;
    GameObject settingsUI = null;
    GameObject mainBackground;
    GameObject blackBackground;
    CanvasGroup lobbyCG;
    CanvasGroup creditsCG;
    CanvasGroup settingsCG;
    
    bool creditsRunning = false;

    Animation anim;
    AudioSource buttonClick;

    GameObject game;
    Settings settings;

    // Use this for initialization
    void Start () {

        lobbyUI = GameObject.Find("LobbyMenu");
        creditsUI = GameObject.Find("Credits");
        settingsUI = GameObject.Find("Settings");
        mainBackground = GameObject.Find("Image");
        blackBackground = GameObject.Find("BlackBack");
        game = GameObject.Find("Persistence");

        lobbyCG = lobbyUI.GetComponent<CanvasGroup>();
        creditsCG = creditsUI.GetComponent<CanvasGroup>();
        settingsCG = settingsUI.GetComponent<CanvasGroup>();

        anim = creditsUI.GetComponent<Animation>();
        buttonClick = this.GetComponent<AudioSource>();

        lobbyCG.alpha = 1.0f;
        lobbyCG.interactable = true;
        lobbyCG.blocksRaycasts = true;

        creditsCG.alpha = 0.0f;
        creditsCG.interactable = false;
        creditsCG.blocksRaycasts = false;

        settingsCG.alpha = 0.0f;
        settingsCG.interactable = false;
        settingsCG.blocksRaycasts = false;

        mainBackground.SetActive(true);
        blackBackground.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("c"))
        {
            runCredits();
        }
        

        if (creditsRunning && !anim.isPlaying)
        {
            stopCredits();
        }
    }


    public void runCredits()
    {
        mainBackground.SetActive(false);
        blackBackground.SetActive(true);
        buttonClick.Play();
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
        mainBackground.SetActive(true);
        blackBackground.SetActive(false);
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
        buttonClick.Play();
        lobbyCG.alpha = 0.0f;
        lobbyCG.interactable = false;
        lobbyCG.blocksRaycasts = false;

        settingsCG.alpha = 1.0f;
        settingsCG.interactable = true;
        settingsCG.blocksRaycasts = true;
    }


    public void hideSettings()
    {
        buttonClick.Play();
        lobbyCG.alpha = 1.0f;
        lobbyCG.interactable = true;
        lobbyCG.blocksRaycasts = true;

        settingsCG.alpha = 0.0f;
        settingsCG.interactable = false;
        settingsCG.blocksRaycasts = false;
    }

    public void exitGame()
    {
        buttonClick.Play();
        Debug.Log("Exiting Game");
        Application.Quit();
        Debug.Break();
    }


    public void saveGame()
    {
        buttonClick.Play();
        Debug.Log("Saving Game");
        game.GetComponent<Persistence>().saveGame();
    }
}
