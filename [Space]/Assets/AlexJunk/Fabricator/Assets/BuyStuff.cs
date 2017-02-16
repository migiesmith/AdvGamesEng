using UnityEngine;
using System.Collections;

public class BuyStuff : MonoBehaviour {

    public GameObject HealBlock;
    public GameObject BloodBlock;
    private GameObject used;
    private GameObject used2;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        used= Instantiate<GameObject>(HealBlock);
        used.transform.position += this.transform.position;
        used2 = Instantiate<GameObject>(BloodBlock);
        used2.transform.position += this.transform.position;
        Debug.Log("Enter");
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        Destroy(used.gameObject);
        Destroy(used2.gameObject);
    }
}
