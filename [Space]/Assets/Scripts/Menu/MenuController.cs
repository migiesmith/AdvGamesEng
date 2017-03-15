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
    CanvasGroup splashCG;
    CanvasGroup mainCG;
    CanvasGroup loadCG;
    bool splashActive = true;

    NVRPlayer player;

    Persistence persistence;

    // Use this for initialization
    void Start() {
        splashUI = GameObject.Find("SplashScreen");
        mainUI = GameObject.Find("MainScreen");
        //Set lobbyUI.
        loadUI = GameObject.Find("LoadingScreen");

        splashCG = splashUI.GetComponent<CanvasGroup>();
        loadCG = loadUI.GetComponent<CanvasGroup>();
        mainCG = mainUI.GetComponent<CanvasGroup>();

        player = GameObject.FindObjectOfType<NVRPlayer>();
        persistence = GameObject.Find("Persistence").GetComponent<Persistence>();

        splashCG.alpha = 1.0f;
        splashCG.interactable = true;
        splashCG.blocksRaycasts = true;

        mainCG.alpha = 0.0f;
        mainCG.interactable = false;
        mainCG.blocksRaycasts = false;

        loadCG.alpha = 0.0f;
        loadCG.interactable = false;
        loadCG.blocksRaycasts = false;

    }

    // Update is called once per frame
    void Update() {
       

    }


    void setLoadingScreen()
    {
        splashCG.alpha = 0.0f;
        splashCG.interactable = false;
        splashCG.blocksRaycasts = false;

        loadCG.alpha = 0.0f;
        loadCG.interactable = false;
        loadCG.blocksRaycasts = false;

        mainCG.alpha = 1.0f;
        mainCG.interactable = true;
        mainCG.blocksRaycasts = true;
    }


    public void setNewGame()
    {
        persistence.newGame();
        loadGame();
    }


    public void showLoadableGames()
    {
        mainCG.alpha = 0.0f;
        mainCG.interactable = false;
        mainCG.blocksRaycasts = false;

        loadCG.alpha = 1.0f;
        loadCG.interactable = true;
        loadCG.blocksRaycasts = true;
    }


    public void loadGame()
    {
        SceneManager.LoadScene(1);
    }

    
    public void exitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
        Debug.Break();
    }


}
