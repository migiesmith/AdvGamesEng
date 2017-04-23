using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public abstract class GameEnemy : Pathfinding
{

    //current behaviour that is in use
    public Behaviour active_behaviour;

    public Weapon weapon;

    //create behaviours
    private PatrolBehaviour patrol;
    private FleeBehaviour flee;
    private CombatBehaviour combat;
    public AlertBehaviour alert;

    //used in combat
    public int health = 100;
    public Transform target;
    public WaypointPathfinder pathFinder;

    [HideInInspector]
    //indicates behaviour state
    public GameObject indicator;

    public Transform player;

    public float detectionRange;

    public ExplosionParticles explosion;

    public Transform weaponTransform;

    public GameObject bullet;

    [HideInInspector]
    public bool alertActive = false;

    [HideInInspector]
    public Vector3 lastKnownLocation;

    public AnimationCurve inertia;
    public AnimationCurve shieldDeactivation;

    public Vector3 velocity;
    public Vector3 lastPos;


    public Renderer rend;

    public float Refire = 0.2f;
    public float RefireDelay = 0.0f;
    public int ammo = 10;
    public float rotationspeed = 100;
    public float aimDampener;

    public AudioClip alertNoise;
    public AudioClip combatNoise;
    public AudioClip patrolNoise;
    public AudioClip deathNoise;

    public AudioSource source;

    private float volLowRange = .5f;
    private float volHighRange = 1.0f;


    public GameObject shield;

    [HideInInspector]
    public Transform defaultShield;

    float vol;

    //time from when die called to robot exploding
    private float timeToExplosion = 1000.0f;

    public Rigidbody rb;

    private space.PlayerState playerState;


    public float timeToLose = 2;

    [HideInInspector]
    public float timeSinceSeen = 0;

    ShieldController sc;

    // Use this for initialization
    virtual
    public void Start()
    {

        //find the indicator
        Transform indTrans = this.transform.FindChild("BehaviourIndicator");
        if (indTrans != null)
            indicator = indTrans.gameObject;

        this.rb = GetComponent<Rigidbody>();

        //initialise behaviours
        patrol = new PatrolBehaviour(this);
        flee = new FleeBehaviour(this);
        combat = new CombatBehaviour(this);
        alert = new AlertBehaviour(this);

        // Get a reference to the WaypointPathfinder
        pathFinder = GameObject.FindObjectOfType<WaypointPathfinder>();


        //set default behaviour to patrol
        ToPatrol();

        this.player = GameObject.FindObjectOfType<NVRHead>().transform;

        weaponTransform = transform.FindChild("Body").FindChild("Gun");


        rend = indicator.GetComponent<Renderer>();

        lastPos = new Vector3(0, 0, 0);
        velocity = new Vector3(0, 0, 0);

        vol = Random.Range(volLowRange, volHighRange);

        defaultShield = shield.transform;

        playerState = player.transform.root.GetComponent<space.PlayerState>();

        sc = shield.GetComponent<ShieldController>();
    }


    //reduce health when hit by weapon
    virtual
    public void TakeDamage(int damage)
    {
        this.health -= damage;
    }

    //switch active behaviour to alert
    public void ToAlert()
    {
        this.active_behaviour = alert;
        
        source.PlayOneShot(alertNoise, vol);
        //indicate current behaviour through colour
        if (indicator != null)
        {

        }
        
    }

    //switch active behaviour to patrol
    public void ToPatrol()
    {
        
        if (sc == null)
        {
            sc = shield.GetComponent<ShieldController>();
        }
        
        
        sc.breakShield();
        //this.shield.SetActive(false);
        this.active_behaviour = patrol;
        source.PlayOneShot(patrolNoise, vol);
        //indicate current behaviour through colour   
        if (indicator != null)
        {
            Renderer r = indicator.GetComponent<Renderer>();
            r.material.SetColor("_Color", Color.green);
        }
    }

    //switch active behaviour to combat
    public void ToCombat()
    {

        if (sc == null)
        {
            sc = shield.GetComponent<ShieldController>();
        }

        sc.reviveShield();

        //this.shield.SetActive(true);
        //Physics.IgnoreCollision(shield.GetComponent<Collider>(), this.GetComponent<Collider>());
        this.active_behaviour = combat;
        source.PlayOneShot(combatNoise, vol);
        //indicate current behaviour through colour
        if (indicator != null)
        {
            Renderer r = indicator.GetComponent<Renderer>();
            r.material.SetColor("_Color", Color.red);
        }

        playerEnterCombat();
    }

    //switch active behaviour to flee
    public void ToFlee()
    {
        this.active_behaviour = flee;

        //indicate current behaviour through colour
        if (indicator != null)
        {
            Renderer r = indicator.GetComponent<Renderer>();
            r.material.SetColor("_Color", Color.blue);
        }
    }

    public void move(float speed)
    {
        if (Path.Count > 0)
        {
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
                //rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            Vector3 lastPos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, Path[0], Time.deltaTime * speed);
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
            
            //rb.velocity = new Vector3(vel.x, 0.0f, vel.z);
            //Debug.Log(rb.velocity);
            transform.LookAt(transform.position + Vector3.Normalize(Path[0] - new Vector3(lastPos.x, Path[0].y ,lastPos.z)));
            if (Vector3.Distance(transform.position, Path[0]) < 2.0f && this.tag != "TutorialBot")
            {
                Path.RemoveAt(0);
            }
            //Vector3 vel = Vector3.Normalize(Path[0] - transform.position) * speed;
            //rb.AddForce(new Vector3(1000.0f, 0, 0.0f), ForceMode.VelocityChange);
        }
    }

    //kill enemy
    public void die()
    {
        if(rb != null)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }

        if (active_behaviour == combat)
            playerExitCombat();

        //source.PlayOneShot(deathNoise, vol);
        if (timeToExplosion <= 0.0f)
        {
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            explosion.play();
            Destroy(this.gameObject);
        }
        else
        {
            timeToExplosion -= Time.fixedTime;
            Debug.Log(Time.fixedTime);
        }

        //Speed up the explosion if necessary.
        if(this.tag == "TutorialBot")
        {
            timeToExplosion -= 1000.0f;
        }
    }

    //fire the gun
    virtual
    public void FireWeapon()
    {
        /*
        if (weaponTransform != null)
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
        */
    }

    public bool checkLineOfSight()
    {
        NVRHead playerObject = GameObject.FindObjectOfType<NVRHead>();
        Vector3 direction = playerObject.transform.position - this.transform.position;
        Debug.DrawRay(this.transform.position, Vector3.Normalize(direction), Color.blue);
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.Normalize(direction), out hitInfo, 10))
        {
            //Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject.name.Equals("DoorSensor"))
            {
                return true;
            }
        }
        return false;
    }

    public void playerEnterCombat()
    {
        if(playerState!=null)
            playerState.newThreat(this.gameObject);
    }

    public void playerExitCombat()
    {
        if (playerState != null)
            playerState.threatOver(this.gameObject);
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sheild")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.gameObject.GetComponent<CapsuleCollider>());
        }

    }
    



    }
