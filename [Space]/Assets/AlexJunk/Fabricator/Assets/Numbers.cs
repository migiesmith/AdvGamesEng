﻿using UnityEngine;
using System.Collections;

public class Numbers : MonoBehaviour {

    public static int money;
    public static int maxHP;
    public static int currHP;
    public GameObject spawner;

	// Use this for initialization
	void Start ()
    {
        money = 100;
        maxHP = 100;
        currHP = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Instantiate(spawner);
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Money: " + money);
        GUILayout.Label("HP: " + currHP + "/" + maxHP);
    }

   

}