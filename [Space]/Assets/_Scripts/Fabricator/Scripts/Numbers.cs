using UnityEngine;
using System.Collections;

public class Numbers : MonoBehaviour {

    //public static int money;

    public static int metals = 0;
    public static int organics = 0;
    public static int fuel = 0;
    public static int radioactive = 0;

    //public static int maxHP;
    //public static int currHP;
    //public GameObject spawner;

    // Use this for initialization
    void Start ()
    {
        //money=metals=organics=fuel = 100;
        //maxHP = 100;
        //currHP = 20;
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("space"))
        //{
        //    Instantiate(spawner);
        //}
    }

    void OnGUI()
    {
       // GUILayout.Label("Money: " + money);
        GUILayout.Label("Metal: " + metals);
        GUILayout.Label("Organics: " + organics);
        GUILayout.Label("Fuel: " + fuel);
        GUILayout.Label("Radioactive: " + radioactive);
        //GUILayout.Label("HP: " + currHP + "/" + maxHP);
    }

   

}
