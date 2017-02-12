using UnityEngine;
using System.Collections;

//[RequireComponent (typeof (SphereCollider)), RequireComponent (typeof (Detection))]
public class Enemy : Pathfinding {

	//current behaviour that is in use
	public Behaviour active_behaviour;

    public Weapon weapon;

	//create behaviours
	private PatrolBehaviour patrol;
	private FleeBehaviour flee;
	private CombatBehaviour combat;
	private AlertBehaviour alert;

	//used in combat
	private int health = 100;
	private Transform target;
	private WaypointPathfinder pathFinder;

    //indicates behaviour state
    private GameObject indicator;

    // Use this for initialization
    void Start () {
	
		//initialise behaviours
		patrol = new PatrolBehaviour (this);
		flee = new FleeBehaviour (this);
		combat = new CombatBehaviour (this);
		alert = new AlertBehaviour (this);

		// Get a reference to the WaypointPathfinder
		pathFinder = GameObject.FindObjectOfType<WaypointPathfinder>();

        //find the indicator
        indicator = GameObject.Find("BehaviourIndicator");

        //set default behaviour to patrol
        ToPatrol();

    }
	
	// Update is called once per frame
	void Update () {

		//update behaviour
		active_behaviour.update ();
	
	}

	//reduce health when hit by weapon
	void TakeDamage(int damage){
		this.health -= damage;
	}

	//switch active behaviour to alert
	public void ToAlert(){
		this.active_behaviour = alert;

		//indicate current behaviour through colour
		if(indicator != null){
			Renderer r = indicator.GetComponent<Renderer> ();
			r.material.SetColor("_Color", Color.yellow);
		}
	}

	//switch active behaviour to patrol
	public void ToPatrol(){
		this.active_behaviour = patrol;

		//indicate current behaviour through colour
		if(indicator != null){
			Renderer r = indicator.GetComponent<Renderer> ();
			r.material.SetColor("_Color", Color.green);
		}
	}

	//switch active behaviour to combat
	public void ToCombat(){
		this.active_behaviour = combat;

		//indicate current behaviour through colour
		if(indicator != null){
			Renderer r = indicator.GetComponent<Renderer> ();
			r.material.SetColor("_Color", Color.red);
		}
	}

	//switch active behaviour to flee
	public void ToFlee(){
		this.active_behaviour = flee;

		//indicate current behaviour through colour
		if(indicator != null){
			Renderer r = indicator.GetComponent<Renderer> ();
			r.material.SetColor("_Color", Color.blue);
		}
	}

	public void move(float speed){		
        if (Path.Count > 0){
            transform.position = Vector3.MoveTowards(transform.position, Path[0], Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, Path[0]) < 0.4f){
                Path.RemoveAt(0);
            }
        }
	} 

}
