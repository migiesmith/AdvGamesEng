using UnityEngine;
using System.Collections;

public class HealScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        Numbers.money -= 80;
        Numbers.currHP = Numbers.maxHP;
        Destroy(this.gameObject);
    }
}
