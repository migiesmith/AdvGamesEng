using UnityEngine;
using System.Collections;

public class BloodScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        Numbers.money += 40;
        Numbers.currHP -= 80;
        Destroy(this.gameObject);
    }
}
