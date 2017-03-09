using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    GameObject splashUI = null;
    GameObject loadUI = null;
    CanvasGroup splashCG;
    CanvasGroup loadCG;
    bool splashActive = true;

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device;

    GameObject player;

    Persistence game;

    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player");
        splashUI = GameObject.Find("SplashScreen");
        //Set lobbyUI.
        loadUI = GameObject.Find("LoadingScreen");

        splashCG = splashUI.GetComponent<CanvasGroup>();
        loadCG = loadUI.GetComponent<CanvasGroup>();

        trackedObject = GetComponent<SteamVR_TrackedObject>();

        splashCG.alpha = 1.0f;
        splashCG.interactable = true;
        splashCG.blocksRaycasts = true;

        loadCG.alpha = 0.0f;
        loadCG.interactable = false;
        loadCG.blocksRaycasts = false;

    }

    // Update is called once per frame
    void Update() {
        device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && splashActive)
        {
            setLoadingScreen();
        }
        else if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && !splashActive)
        {
            Trigger();
        }

    }


    void Trigger()
    {
        RaycastHit seen;
        Ray direction = new Ray(player.transform.position, player.transform.forward);
       /* if (Physics.Raycast(direction, out seen, 5.0f))
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

        }*/
        Debug.DrawRay(transform.position, transform.forward, Color.black, 1);
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


    public void loadGame()
    {
        SceneManager.LoadScene(1);
    }

    
    public void exitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }


}
