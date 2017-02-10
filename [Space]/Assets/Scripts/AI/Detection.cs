using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SphereCollider))]
public class Detection : MonoBehaviour {

	private SphereCollider triggerSphere;

	// Use this for initialization
	void Start () {
		this.triggerSphere = this.GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collided){
		
		if (collided.gameObject.tag.Equals("Player")) {
            Debug.Log("Collided with player");
			Enemy enemy = (Enemy)transform.parent.gameObject.GetComponent<Enemy>();
			enemy.ToAlert();
			AlertBehaviour ab = (AlertBehaviour)enemy.active_behaviour;
			ab.SetRotation (360);
		}
	}
}
