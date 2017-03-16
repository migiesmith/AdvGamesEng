using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NewtonVR;

public class MenuController : MonoBehaviour {

    GameObject splashUI = null;
    GameObject loadUI = null;
    GameObject mainUI = null;
    GameObject errorUI = null;
    GameObject checkUI = null;
    CanvasGroup splashCG;
    CanvasGroup mainCG;
    CanvasGroup loadCG;
    CanvasGroup errorCG;
    CanvasGroup checkCG;
    bool splashActive = true;
    int tempIndex;

    NVRPlayer player;
    AudioSource buttonClick;

    GameObject game;

    // Use this for initialization
    void Start() {
        splashUI = GameObject.Find("SplashScreen");
        mainUI = GameObject.Find("MainScreen");
        //Set lobbyUI.
        loadUI = GameObject.Find("LoadingScreen");
        errorUI = GameObject.Find("ErrorScreen");
        checkUI = GameObject.Find("DeleteScreen");

        splashCG = splashUI.GetComponent<CanvasGroup>();
        loadCG = loadUI.GetComponent<CanvasGroup>();
        mainCG = mainUI.GetComponent<CanvasGroup>();
        errorCG = mainUI.GetComponent<CanvasGroup>();
        checkCG = mainUI.GetComponent<CanvasGroup>();

        buttonClick = this.GetComponent<AudioSource>();
        player = GameObject.FindObjectOfType<NVRPlayer>();
        //game = GameObject.Find("Persistence");

        splashCG.alpha = 1.0f;
        splashCG.interactable = true;
        splashCG.blocksRaycasts = true;

        mainCG.alpha = 0.0f;
        mainCG.interactable = false;
        mainCG.blocksRaycasts = false;

        loadCG.alpha = 0.0f;
        loadCG.interactable = false;
        loadCG.blocksRaycasts = false;

        errorCG.alpha = 0.0f;
        errorCG.interactable = false;
        errorCG.blocksRaycasts = false;

        checkCG.alpha = 0.0f;
        checkCG.interactable = false;
        checkCG.blocksRaycasts = false;

    }

    // Update is called once per frame
    void Update() {

        if (player.RightHand.Inputs[NVRButtons.Trigger].IsPressed && splashActive)
        {
            splashActive = false;
            setLoadingScreen();
        }
    }


    public void setLoadingScreen()
    {
        buttonClick.Play();
        splashCG.alpha = 0.0f;
        splashCG.interactable = false;
        splashCG.blocksRaycasts = false;

        loadCG.alpha = 0.0f;
        loadCG.interactable = false;
        loadCG.blocksRaycasts = false;

        checkCG.alpha = 0.0f;
        checkCG.interactable = false;
        checkCG.blocksRaycasts = false;

        errorCG.alpha = 0.0f;
        errorCG.interactable = false;
        errorCG.blocksRaycasts = false;

        mainCG.alpha = 1.0f;
        mainCG.interactable = true;
        mainCG.blocksRaycasts = true;
    }


    public void setNewGame()
    {
        buttonClick.Play();
        bool check = game.GetComponent<Persistence>().newGame();
        if (!check)
        {
            displayError();
        }
        else
        {
            changeScene();
        }
    }


    public void showLoadableGames()
    {
        buttonClick.Play();
        mainCG.alpha = 0.0f;
        mainCG.interactable = false;
        mainCG.blocksRaycasts = false;

        loadCG.alpha = 1.0f;
        loadCG.interactable = true;
        loadCG.blocksRaycasts = true;
    }


    public void displayError()
    {
        mainCG.alpha = 0.0f;
        mainCG.interactable = false;
        mainCG.blocksRaycasts = false;

        errorCG.alpha = 1.0f;
        errorCG.interactable = true;
        errorCG.blocksRaycasts = true;
    }


    public void displayCheckScreen()
    {

        //TODO set tempindex
        buttonClick.Play();

        loadCG.alpha = 0.0f;
        loadCG.interactable = false;
        loadCG.blocksRaycasts = false;

        checkCG.alpha = 1.0f;
        checkCG.interactable = true;
        checkCG.blocksRaycasts = true;
    }


    public void loadGame()
    {
        buttonClick.Play();
        //TODO get index.
        game.GetComponent<Persistence>().loadGame(tempIndex);
    }


    public void changeScene()
    {
        this.GetComponent<SteamVR_LoadLevel>().Trigger();
    }


    public void deleteSaveFile()
    {
        buttonClick.Play();
        game.GetComponent<Persistence>().deleteSaveFile(tempIndex);
        setLoadingScreen();
    }


    public void exitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
        Debug.Break();
    }


}
