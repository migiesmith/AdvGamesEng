using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    GameObject splashUI = null;
    GameObject loadUI = null;
    CanvasGroup splashCG;
    CanvasGroup loadCG;
    bool splashActive = true;

    NewtonVR.NVRHand[] hand;
    

    // Use this for initialization
    void Start () {
        splashUI = GameObject.Find("SplashScreen");
        //Set lobbyUI.
        loadUI = GameObject.Find("LoadingScreen");

        hand = GameObject.Find("Player").GetComponentsInChildren<NewtonVR.NVRHand>();

        splashCG = splashUI.GetComponent<CanvasGroup>();
        loadCG = loadUI.GetComponent<CanvasGroup>();

        splashCG.alpha = 1.0f;
        splashCG.interactable = true;
        splashCG.blocksRaycasts = true;

        loadCG.alpha = 0.0f;
        loadCG.interactable = false;
        loadCG.blocksRaycasts = false;

    }
	
	// Update is called once per frame
	void Update () {
        while (splashActive)
        {
            if(hand[0].HoldButtonDown || hand[1].HoldButtonDown)
            {
                splashActive = false;
                setLoadingScreen();
            }
        }
	}


    void setLoadingScreen()
    {
        splashCG.alpha = 0.0f;
        splashCG.interactable = false;
        splashCG.blocksRaycasts = false;

        loadCG.alpha = 1.0f;
        loadCG.interactable = true;
        loadCG.blocksRaycasts = true;
    }


    public void setNewGame()
    {

    }


    public void showLoadableGames()
    {

    }

    
    public void exitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }
}
