using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop2 : MonoBehaviour {

    public GameObject[] itemList;
    static public int curr = 0;
    public space.ItemSpawn spawner;
    private ShopValues currVals;
    private Currency playerVals;

    // Use this for initialization
    void Start()
    {
        curr = 0;
        playerVals = FindObjectOfType<Currency>();
        updateSelection();
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
            GetComponent<Renderer>().material = currVals.image;
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
    }
}
