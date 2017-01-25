using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : MonoBehaviour {

	public int points = 14;

	public GameObject bulletPrefab;

	public float strength = 10.0f;

	public NewtonVR.NVRHand hand;

	public Transform toMove;

	private LineRenderer lineRend;
	private GameObject bullet = null;

	// Use this for initialization
	void Start () {
		lineRend = this.GetComponent<LineRenderer>();

	}

	// Update is called once per frame
	void Update () {
		if(this.hand.UseButtonDown){
			teleport();
		}
	}

	public void teleport(){
		if(bullet == null){
			bullet = (GameObject) Instantiate(bulletPrefab);
			bullet.transform.position = hand.transform.position;
			bullet.GetComponent<Rigidbody>().AddForce(hand.transform.forward * strength);

			TeleportCollide bulletScript = bullet.GetComponent<TeleportCollide>();
			bulletScript.toTeleport = toMove;			
		}
	}

}
