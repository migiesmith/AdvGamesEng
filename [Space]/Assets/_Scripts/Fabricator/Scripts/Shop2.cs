using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop2 : MonoBehaviour {

    public GameObject[] itemList;
    static public int curr = 0;
/*
    public GameObject InfoBoxPrefab;
    private GameObject InfoBox;
    public GameObject UpBoxPrefab;
    private GameObject UpBox;
    public GameObject DownBoxPrefab;
    private GameObject DownBox;
    public GameObject BuyBoxPrefab;
    private GameObject BuyBox;*/

    private space.ItemSpawn spawner;
    private ShopValues currVals;
    private Currency playerVals;

    // Use this for initialization
    void Start()
    {
        /*
        InfoBox = Instantiate<GameObject>(InfoBoxPrefab);  
        InfoBox.transform.position += this.transform.position;
           
        UpBox = Instantiate<GameObject>(UpBoxPrefab);
        DownBox = Instantiate<GameObject>(DownBoxPrefab);
        BuyBox= Instantiate<GameObject>(BuyBoxPrefab);

        UpBox.transform.position += this.transform.position;
        DownBox.transform.position += this.transform.position;
        BuyBox.transform.position += this.transform.position;
        */

        spawner = GetComponent<space.ItemSpawn>();
        playerVals = FindObjectOfType<Currency>();
    }

    public void up()
    {
        curr--;
        if(curr <= -1)
        {
            curr = itemList.Length - 1;
        }
        updateSelection();
    }

    public void down()
    {  
        curr++;
        if (curr >= itemList.Length)
        {
            curr = 0;
        }
        updateSelection();
    }

    public void updateSelection()
    {
        currVals = itemList[curr].GetComponent<ShopValues>();
        if (currVals != null)
        {
            //string metalsString = currVals.metals.ToString();
            //string orgsString = currVals.organics.ToString();
            //string fuelString = currVals.fuel.ToString();

            GetComponent<Renderer>().material = currVals.image;
            //InfoBox.GetComponent<TextMesh>().text = "Cost: \nMetal: " + metalsString + "\nOrganics: " + orgsString + "\nFuel: " + fuelString + "\nDescription: " + currVals.description;

            spawner.updateDetails(itemList[curr]);
        }
    }

    public void spawn()
    {
        List<int> available = playerVals.getCurrency();
        List<int> required = currVals.getVals();
        if (available[0] >= required[0] && available[1] >= required[1] && available[2] >= required[2] && available[3] >= required[3])
        {
            currVals.buy();
            spawner.spawn(itemList[curr]);
        }
        //GameObject item = Instantiate(itemList[curr], this.transform.position- new Vector3(-0.1f, Random.Range(0.0f, 1.0f), Random.Range(-1.0f, 1.0f)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update ()
    {       
        
    }
}
