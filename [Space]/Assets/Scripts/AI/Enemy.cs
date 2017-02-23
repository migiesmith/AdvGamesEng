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

    [HideInInspector]
    //indicates behaviour state
    public GameObject indicator;

    public GameObject player;

    public float detectionRange;

    public GameObject explosion;

    Transform weaponTransform;

    public GameObject bullet;

    public float Refire = 0.2f;
    private float RefireDelay = 0.0f;
    public int ammo = 10;
    public float rotationspeed = 100;
    public float aimDampener;


    Renderer rend;

    // Use this for initialization
    void Start ()
    {
        //find the indicator
        Transform indTrans = this.transform.FindChild("BehaviourIndicator");
        if (indTrans != null)
            indicator = indTrans.gameObject;

        //initialise behaviours
        patrol = new PatrolBehaviour (this);
		flee = new FleeBehaviour (this);
		combat = new CombatBehaviour (this);
		alert = new AlertBehaviour (this);

        // Get a reference to the WaypointPathfinder
        pathFinder = GameObject.FindObjectOfType<WaypointPathfinder>();


        //set default behaviour to patrol
        ToPatrol();

        this.player = GameObject.FindGameObjectWithTag("Player");

        weaponTransform = transform.FindChild("Body").FindChild("Gun");
     

        rend = indicator.GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void Update () {
		//update behaviour
		active_behaviour.update ();
	}


    void FixedUpdate()
    {
        Vector3 player_pos = player.transform.position;
        //Debug.Log(Vector3.Distance(this.transform.position, player_pos));
        if (Vector3.Distance(this.transform.position, player_pos) <= detectionRange && this.active_behaviour == this.patrol && this.alert.active)
        {
            this.alert.SetRotation(360);
            ToAlert();
            this.alert.active = true;
        }
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

    //kill enemy
    public void die()
    {
        
        Instantiate(explosion, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }

    //fire the gun
    public void FireWeapon()
    {
        if(weaponTransform != null)
        {
            if (RefireDelay <= 0)
            {
                RefireDelay = Refire;
                Instantiate(bullet, this.transform.position, this.transform.rotation);
                ammo--;
            }
            else if (RefireDelay > 0)
                RefireDelay -= Time.deltaTime;
        }
    }

}
