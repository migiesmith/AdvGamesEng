using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {

    GameObject datapad;

    AudioSource buttonClick;
    List<GameObject> lootInv;
    List<int> currency;

    int inventoryIndex = 0;

    // Use this for initialization
    void Start () {
        datapad = GameObject.Find("Datapad");
        lootInv = FindObjectOfType<LootInventory>().getLoot();
        currency = FindObjectOfType<Currency>().getCurrency();
        buttonClick = this.GetComponent<AudioSource>();

        GameObject.Find("pageCount").gameObject.GetComponent<Text>().text = "1/" + lootInv.Count;
        GameObject.Find("metalTotal").gameObject.GetComponent<Text>().text = "Metal \n" + currency[0];
        GameObject.Find("organicTotal").gameObject.GetComponent<Text>().text = "Organic \n" + currency[1];
        GameObject.Find("fuelTotal").gameObject.GetComponent<Text>().text = "Fuel \n" + currency[2];
        GameObject.Find("radioactiveTotal").gameObject.GetComponent<Text>().text = "RA \n" + currency[3];

        this.transform.FindChild("errorText").gameObject.SetActive(false);
        changeInventory();
    }
	
	// Update is called once per frame
	void Update () {
        datapad.transform.position = datapad.transform.parent.transform.position;
        datapad.transform.rotation = datapad.transform.parent.transform.rotation;
        this.transform.position = datapad.transform.position;
        this.transform.rotation = datapad.transform.rotation;
        Debug.Log(datapad.transform.parent.gameObject.name);
    }


    public void previousInventory()
    {
        buttonClick.Play();
        if (inventoryIndex == 0)
        {
            inventoryIndex = lootInv.Count - 1;
            changeInventory();
        }
        else
        {
            for (int i = 0; i < lootInv.Count; i++)
            {
                if (inventoryIndex == i)
                {
                    inventoryIndex = i-1;
                    changeInventory();
                    break;
                }
            }
        }
    }


    public void nextInventory()
    {
        buttonClick.Play();
        if (inventoryIndex == (lootInv.Count-1))
        {
            inventoryIndex = 0;
            changeInventory();
        }
        else
        {
            for (int i = 0; i < lootInv.Count; i++)
            {
                if (inventoryIndex == i)
                {
                    inventoryIndex = i+1;
                    changeInventory();
                    break;
                }
            }
        }
    }


    public void changeInventory()
    {
        if (checkLoot())
        {
            GameObject.Find("nameText").GetComponent<Text>().text = lootInv[inventoryIndex].name;
            GameObject.Find("metalText").GetComponent<Text>().text = "Metal: " + Numbers.metals;
            GameObject.Find("organicText").GetComponent<Text>().text = "Organic: " + Numbers.organics;
            GameObject.Find("fuelText").GetComponent<Text>().text = "Fuel: " + Numbers.fuel;
            GameObject.Find("radioactiveText").GetComponent<Text>().text = "Radioactive: " + Numbers.radioactive;
            int tempIndex = inventoryIndex + 1;
            GameObject.Find("pageCount").GetComponent<Text>().text = tempIndex + "/" + lootInv.Count;

            //Set Image
            GameObject.Find("lootImage").GetComponent<Renderer>().material = lootInv[inventoryIndex].GetComponent<ShopValues>().image;
        }
    }


    public bool checkLoot()
    {
        if (lootInv.Count < 1)
        {
            this.transform.FindChild("errorText").gameObject.SetActive(true);

            this.transform.FindChild("nextButton").gameObject.SetActive(false);
            this.transform.FindChild("previousButton").gameObject.SetActive(false);
            this.transform.FindChild("dropButton").gameObject.SetActive(false);

            this.transform.FindChild("MetalAmount").gameObject.SetActive(false);
            this.transform.FindChild("OrganicAmount").gameObject.SetActive(false);
            this.transform.FindChild("FuelAmount").gameObject.SetActive(false);
            this.transform.FindChild("RadioactiveAmount").gameObject.SetActive(false);
            this.transform.FindChild("metalTotal").gameObject.SetActive(false);
            this.transform.FindChild("organicTotal").gameObject.SetActive(false);
            this.transform.FindChild("fuelTotal").gameObject.SetActive(false);
            this.transform.FindChild("radioactiveTotal").gameObject.SetActive(false);

            this.transform.FindChild("nameText").gameObject.SetActive(false);
            this.transform.FindChild("metalText").gameObject.SetActive(false);
            this.transform.FindChild("organicText").gameObject.SetActive(false);
            this.transform.FindChild("fuelText").gameObject.SetActive(false);
            this.transform.FindChild("radioactiveText").gameObject.SetActive(false);

            this.transform.FindChild("lootImage").gameObject.SetActive(false);
            this.transform.FindChild("pageCount").gameObject.SetActive(false);

            return false;
        }
        else if (lootInv.Count == 1)
        {
            this.transform.FindChild("errorText").gameObject.SetActive(false);
            this.transform.FindChild("nextButton").gameObject.SetActive(false);
            this.transform.FindChild("previousButton").gameObject.SetActive(false);
            return true;
        }
        else
        {
            this.transform.FindChild("errorText").gameObject.SetActive(false);
            this.transform.FindChild("nextButton").gameObject.SetActive(true);
            this.transform.FindChild("previousButton").gameObject.SetActive(true);
            return true;
        }
    }


    public void dropLoot()
    {
        FindObjectOfType<LootInventory>().dropLoot(inventoryIndex);
        updateExtra();
    }


    public void openInventory() //Pretty sure this won't work.
    {
        this.transform.gameObject.SetActive(false);
    }

    public void closeInventory()
    {
        this.transform.gameObject.SetActive(false);
    }


    public void updateExtra()
    {
        buttonClick.Play();
        inventoryIndex = 0;
        //this.transform.FindChild("pageCount").gameObject.GetComponent<Text>().text
        GameObject.Find("metalTotal").gameObject.GetComponent<Text>().text = "Metal \n" + currency[0];
        GameObject.Find("organicTotal").gameObject.GetComponent<Text>().text = "Organic \n" + currency[1];
        GameObject.Find("fuelTotal").gameObject.GetComponent<Text>().text = "Fuel \n" + currency[2];
        GameObject.Find("radioactiveTotal").gameObject.GetComponent<Text>().text = "RA \n" + currency[3];
        changeInventory();
    }
}
