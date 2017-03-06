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
	public AlertBehaviour alert;

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

    [HideInInspector]
    public bool alertActive = false;

    [HideInInspector]
    public Vector3 lastKnownLocation;

    public AnimationCurve inertia;

    Vector3 velocity;
    Vector3 lastPos;


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

        lastPos = new Vector3(0, 0, 0);
        velocity = new Vector3(0, 0, 0);

    }
	
	// Update is called once per frame
	void Update () {
        
    }


    void FixedUpdate()
    {
        //update behaviour
        lastPos = this.transform.position;
        active_behaviour.update();

       // velocity = new Vector3(Mathf.Abs(lastPos.x - this.transform.position.x), Mathf.Abs(lastPos.y - this.transform.position.y), Mathf.Abs(lastPos.z - this.transform.position.z));
        //if(velocity.x < 0.025f)
        //{
        //    velocity.x = 0.0f;
        //}
        //velocity.y = 0.0f;
        //this.transform.rotation *= Quaternion.Euler(velocity * 5.0f);
        //if(velocity.z>0.0f)
        //    Debug.Log(velocity.z);

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
