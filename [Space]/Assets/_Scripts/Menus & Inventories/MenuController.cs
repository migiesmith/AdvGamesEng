using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NewtonVR;
using UnityEngine.UI;
using System.Linq;

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

    Dictionary<int, string> loadedGames;
    int tempIndex = 0;

    public NVRPlayer player;
    public AudioSource buttonClick;
    

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
        errorCG = errorUI.GetComponent<CanvasGroup>();
        checkCG = checkUI.GetComponent<CanvasGroup>();

        buttonClick = this.GetComponent<AudioSource>();
        player = GameObject.FindObjectOfType<NVRPlayer>();
        game = GameObject.Find("Persistence");

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

        if(loadedGames == null || loadedGames.Count < 1)
        {
            loadedGames = game.GetComponent<Persistence>().getSavedFiles();
        }

        if (loadedGames.Count < 1)
        {
            loadUI.transform.FindChild("errorText").gameObject.SetActive(true);
            loadUI.transform.FindChild("IndexText").gameObject.SetActive(false);
            loadUI.transform.FindChild("TimeText").gameObject.SetActive(false);
            loadUI.transform.FindChild("nextButton").gameObject.SetActive(false);
            loadUI.transform.FindChild("previousButton").gameObject.SetActive(false);
            loadUI.transform.FindChild("loadButton").gameObject.SetActive(false);
            loadUI.transform.FindChild("deleteButton").gameObject.SetActive(false);
            loadUI.transform.FindChild("returnButton").gameObject.SetActive(true);
        }
        else if (loadedGames.Count == 1)
        {
            loadUI.transform.FindChild("nextButton").gameObject.SetActive(false);
            loadUI.transform.FindChild("previousButton").gameObject.SetActive(false);
            loadUI.transform.FindChild("errorText").gameObject.SetActive(false);
            tempIndex = loadedGames.Keys.ElementAt(0);
            changeLoad();
        }
        else
        {
            loadUI.transform.FindChild("errorText").gameObject.SetActive(false);
            tempIndex = loadedGames.Keys.ElementAt(0);
            changeLoad();
        }
    }


    public void previousLoad()
    {
        if (tempIndex == loadedGames.Keys.ElementAt(0))
        {
            tempIndex = loadedGames.Keys.ElementAt(loadedGames.Count - 1);
            changeLoad();
        }
        else
        {
            for (int i = 0; i < loadedGames.Count; i++)
            {
                if (tempIndex == loadedGames.Keys.ElementAt(i))
                {
                    tempIndex = loadedGames.Keys.ElementAt(i - 1);
                    changeLoad();
                    break;
                }
            }
        }
    }

    public void nextLoad()
    {
        if(tempIndex == loadedGames.Keys.ElementAt(loadedGames.Count - 1))
        {
            tempIndex = loadedGames.Keys.ElementAt(0);
            changeLoad();
        }
        else
        {
            for(int i=0; i < loadedGames.Count; i++)
            {
                if(tempIndex == loadedGames.Keys.ElementAt(i))
                {
                    tempIndex = loadedGames.Keys.ElementAt(i + 1);
                    changeLoad();
                    break;
                }
            }
        }
    }


    public void changeLoad()
    {
        Debug.Log("Index: " + tempIndex);
        GameObject.Find("IndexText").GetComponent<Text>().text = "Save File: " + tempIndex;
        GameObject.Find("TimeText").GetComponent<Text>().text = loadedGames[tempIndex];
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
        game.GetComponent<Persistence>().loadGame(tempIndex);
        changeScene();
    }


    public void changeScene()
    {
        activatePlayerComponents();
        bool done = game.GetComponent<Persistence>().tutorialDone;
        foreach (SteamVR_LoadLevel level in this.GetComponents<SteamVR_LoadLevel>()){
            if (!done && level.levelName == "Tutorial")
            {
                level.Trigger();
                break;
            }
            else if(done && level.levelName == "Ship")
            {
                level.Trigger();
                break;
            }
        }
    }

    // Added this so we can use the same player prefab throughout by disabling
    // certain features in the menu by default and reenabling them on load - Robert.
    public void activatePlayerComponents()
    {
        player.GetComponent<space.Dash2>().enabled = true;
        player.GetComponentInChildren<space.WeaponSlotWrapper>().locked = false;
        player.GetComponent<NVRCanvasInput>().NormalCursorScale = 0.05f;
        DontDestroyOnLoad(player);
    }

    public void deleteSaveFile()
    {
        buttonClick.Play();
        game.GetComponent<Persistence>().deleteSaveFile(tempIndex);
        loadedGames.Remove(tempIndex);
        setLoadingScreen();
    }


    public void exitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
        Debug.Break();
    }


}
