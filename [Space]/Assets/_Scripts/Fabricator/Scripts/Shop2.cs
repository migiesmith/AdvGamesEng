using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop2 : MonoBehaviour {

    public GameObject[] itemList;
    static public int curr = 0;
    public GameObject InfoBoxPrefab;
    private GameObject InfoBox;
/*    public GameObject UpBoxPrefab;
    private GameObject UpBox;
    public GameObject DownBoxPrefab;
    private GameObject DownBox;
    public GameObject BuyBoxPrefab;
    private GameObject BuyBox;*/

    public Transform spawnLocation;
    private space.ConsumableInventory consumableInventory;
    public GameObject[] ammoItems;
    public Dictionary<string, GameObject> ammoList = new Dictionary<string, GameObject>();
    private GameObject currAmmo;
    public GameObject ammoButton;
    public TextMesh itemDescription;
    public TextMesh weaponStats;
    public TextMesh ammoCount;
    private Gradient dissolveGradient = new Gradient();
    public Texture dissolveTex;

    void Awake()
    {
        foreach (GameObject g in ammoItems)
            ammoList.Add(g.name, g);
        consumableInventory = FindObjectOfType<space.ConsumableInventory>();
        ammoButton.SetActive(false);
        itemDescription.text = "";
//      weaponStats.text = "";
        ammoCount.text = "";

        GradientColorKey[] dgColor = new GradientColorKey[3];
        dgColor[0].color = Color.white;
        dgColor[0].time = 0.0f;
        dgColor[1].color = Color.cyan;
        dgColor[1].time = 0.5f;
        dgColor[2].color = Color.white;
        dgColor[2].time = 1.0f;
        GradientAlphaKey[] dgAlpha = new GradientAlphaKey[2];
        dgAlpha[0].alpha = 0.0f;
        dgAlpha[0].time = 0.0f;
        dgAlpha[1].alpha = 1.0f;
        dgAlpha[1].time = 1.0f;
        dissolveGradient.SetKeys(dgColor, dgAlpha);
    }

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

        updateDetails();
    }

    public void spawn()
    {
        itemList[curr].GetComponent<ShopValues>().buy();
        //GameObject item = Instantiate(itemList[curr], this.transform.position- new Vector3(-0.1f, Random.Range(0.0f, 1.0f), Random.Range(-1.0f, 1.0f)), Quaternion.identity);

        if (consumableInventory.inventoryList.ContainsKey(itemList[curr]))
        {
            ++consumableInventory.inventoryList[itemList[curr]];
            consumableInventory.updateCount();
        }
        else
        {
            GameObject item = Instantiate(itemList[curr], spawnLocation.position, Quaternion.identity);
            item.name = itemList[curr].name;

            Dissolve dissolve = item.AddComponent<Dissolve>();
            dissolve.colorOverLife = dissolveGradient;
            dissolve.dissolveTex = dissolveTex;
            dissolve.dissolveIn();

            Rigidbody itemRB = item.GetComponentInChildren<Rigidbody>();
            if (itemRB != null)
            {
                itemRB.useGravity = false;
                itemRB.isKinematic = true;
            }
        }
    }

    public void buyAmmo()
    {
        if (consumableInventory.inventoryList.ContainsKey(currAmmo))
        {
            currAmmo.GetComponent<ShopValues>().buy();
            ++consumableInventory.inventoryList[currAmmo];
            ammoCount.text = consumableInventory.inventoryList[currAmmo].ToString();
        }          
    }

    public void updateDetails()
    {
        itemDescription.text = itemList[curr].GetComponent<ShopValues>().description;
        if (itemList[curr].GetComponent<space.Reloadable>())
        {
            currAmmo = ammoList[itemList[curr].name + "_Magazine"];
            ammoButton.SetActive(true);
            ammoCount.text = consumableInventory.inventoryList[currAmmo].ToString();
        }
        else
        {
            currAmmo = null;
            ammoButton.SetActive(false);
            ammoCount.text = "";
        }
    }

	// Update is called once per frame
	void Update ()
    {       
        
    }



}
