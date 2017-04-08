using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SphereCollider))]
public class Detection : MonoBehaviour {

	private SphereCollider triggerSphere;
    private GameObject player;

	// Use this for initialization
	void Start () {
		this.triggerSphere = this.GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void FixedUpdate()
    {
        
    }


    void OnTriggerEnter(Collider collided){
        Debug.Log(collided.gameObject.tag);
        if (collided.gameObject.tag.Equals("PlayerCollider")) {
            Debug.Log("Collided with player");
			Enemy enemy = (Enemy)transform.parent.gameObject.GetComponent<Enemy>();
			enemy.ToAlert();
			AlertBehaviour ab = (AlertBehaviour)enemy.active_behaviour;
			ab.SetRotation (360);
		}
	}
}
