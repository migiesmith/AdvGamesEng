using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop2 : MonoBehaviour {

    public GameObject[] itemList;
    static public int curr = 0;
    public GameObject InfoBoxPrefab;
    private GameObject InfoBox;
    public GameObject UpBoxPrefab;
    private GameObject UpBox;
    public GameObject DownBoxPrefab;
    private GameObject DownBox;
    public GameObject BuyBoxPrefab;
    private GameObject BuyBox;
    // Use this for initialization
    void Start()
    {
        InfoBox = Instantiate<GameObject>(InfoBoxPrefab);  
        InfoBox.transform.position += this.transform.position;
        /*     
        UpBox = Instantiate<GameObject>(UpBoxPrefab);
        DownBox = Instantiate<GameObject>(DownBoxPrefab);
        BuyBox= Instantiate<GameObject>(BuyBoxPrefab);

        UpBox.transform.position += this.transform.position;
        DownBox.transform.position += this.transform.position;
        BuyBox.transform.position += this.transform.position;
        */
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
        string metalsString = itemList[curr].GetComponent<ShopValues>().metals.ToString();
        string orgsString = itemList[curr].GetComponent<ShopValues>().organics.ToString();
        string fuelString = itemList[curr].GetComponent<ShopValues>().fuel.ToString();

        GetComponent<Renderer>().material = itemList[curr].GetComponent<ShopValues>().image;
        InfoBox.GetComponent<TextMesh>().text ="Cost: \nMetal: "+ metalsString + "\nOrganics: "+ orgsString+ "\nFuel: " + fuelString + "\nDescription: " + itemList[curr].GetComponent<ShopValues>().description;
    }

    public void spawn()
    {
            itemList[curr].GetComponent<ShopValues>().buy();
            GameObject item = Instantiate(itemList[curr], this.transform.position- new Vector3(-0.1f, Random.Range(0.0f, 1.0f), Random.Range(-1.0f, 1.0f)), Quaternion.identity);
            item.name = itemList[curr].name;
    }

	// Update is called once per frame
	void Update ()
    {       
        
    }



}
