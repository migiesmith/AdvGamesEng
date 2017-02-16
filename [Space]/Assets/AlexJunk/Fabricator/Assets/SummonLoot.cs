using UnityEngine;
using System.Collections;

public class SummonLoot : MonoBehaviour {

    public GameObject[] lootItems;
    public int[] lootRates;
    int lootTotal=0;

	// Use this for initialization
	void Start ()
    {
        foreach(int i in lootRates)
        {
            lootTotal += i;
        }
        int whichItem=(int)Random.Range(0.0f,lootTotal);
        int count=0;
        while (whichItem > 0)
        {
            whichItem -= lootRates[count];
            if (whichItem <= 0)
            {
                Instantiate(lootItems[count], new Vector3(Random.Range(-14.0f, -4.0f), 1, Random.Range(-9.0f, -1.0f)), Quaternion.identity);
                Destroy(this.gameObject);
            }
            else count++;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
