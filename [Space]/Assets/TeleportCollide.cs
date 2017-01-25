using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeleportCollide : MonoBehaviour {

	private int points = 10;
	private LineRenderer lineRend;

	public Transform toTeleport;

	public float lifeTime = 5.0f;


	// Use this for initialization
	void Start () {
		lineRend = this.gameObject.GetComponent<LineRenderer>();
		lineRend.numPositions = points;

		for(int i = 0; i < points; i++)
			lineRend.SetPosition(i, this.transform.position);

	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(this.transform.position, lineRend.GetPosition(0)) > 0.2f){
			for(int i = points - 1; i > 0; i--){
				lineRend.SetPosition(i, lineRend.GetPosition(i-1));
				Debug.Log(i);
			}
			Debug.Log("\n");
			lineRend.SetPosition(0, this.transform.position);
		}

		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0.0f)
			Destroy(this.gameObject);
	}

	void OnCollisionEnter(Collision collision){
		NavMeshHit navHit;
		foreach (ContactPoint contact in collision.contacts) {
			if(NavMesh.SamplePosition(contact.point, out navHit, 0.1f, NavMesh.AllAreas)){
				toTeleport.position = contact.point;
				Destroy(this.gameObject);
				return;
			}
        }
	}
	void OnCollisionStay(Collision collision){
		NavMeshHit navHit;
		foreach (ContactPoint contact in collision.contacts) {
			if(NavMesh.SamplePosition(contact.point, out navHit, 0.1f, NavMesh.AllAreas)){
				toTeleport.position = contact.point;
				Destroy(this.gameObject);
				break;
			}
        }
	}
}
