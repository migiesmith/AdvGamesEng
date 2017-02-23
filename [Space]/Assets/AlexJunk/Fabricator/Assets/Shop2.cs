using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop2 : MonoBehaviour {

    public GameObject[] itemList;
    static public int curr = 0;
    static public bool spawn = false;
    public GameObject InfoBoxPrefab;
    private GameObject InfoBox;
    public GameObject UpBoxPrefab;
    private GameObject UpBox;
    public GameObject DownBoxPrefab;
    private GameObject DownBox;
    // Use this for initialization
    void Start()
    {
        InfoBox = Instantiate<GameObject>(InfoBoxPrefab);
        UpBox = Instantiate<GameObject>(UpBoxPrefab);
        DownBox = Instantiate<GameObject>(DownBoxPrefab);
    }

	// Update is called once per frame
	void Update ()
    {            
        if (curr >= itemList.Length)
        {
            curr = 0;
        }
        else if(curr <= -1)
        {
            curr = itemList.Length - 1;
        }

        string priceString = itemList[curr].GetComponent<ShopValues>().price.ToString();
        GetComponent<Renderer>().material = itemList[curr].GetComponent<ShopValues>().image;
        InfoBox.GetComponent<TextMesh>().text ="Cost: "+ priceString+"\nDescription: " + itemList[curr].GetComponent<ShopValues>().description;

        if (spawn)
        {
            Numbers.money-= itemList[curr].GetComponent<ShopValues>().price;
            Instantiate(itemList[curr], new Vector3(Random.Range(-14.0f, -4.0f), 1, Random.Range(-9.0f, -1.0f)), Quaternion.identity);
            spawn = false;
        }
    }

}
