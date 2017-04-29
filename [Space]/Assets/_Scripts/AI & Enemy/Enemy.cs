using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using space;

[RequireComponent (typeof (LineRenderer))]
public class Enemy : GameEnemy {

    public List<Transform> barrels;
    private LineRenderer tracer;
    public LayerMask gunLayerMask;
    public float weaponDamage = 10.0f;

    private float timeForDeactivation = 2.0f;
    private float reloadTime = 3.0f;

    private bool reloadNoiseActive = true;
    

    override
    public void Start()
    {
        base.Start();
        
        tracer = GetComponent<LineRenderer>();
        tracer.numPositions = 2;
        tracer.enabled = false;
        Debug.Log(name +": "+ tracer);
    }
    
	// Update is called once per frame
	void Update () {
        
    }

    void FixedUpdate()
    {
        //this.rb.AddForce(0.0f, 1.0f, 0.0f);
        //this.rb.velocity = new Vector3(10.0f, 0.0f, 0.0f);
        //update behaviour
        //lastPos = this.transform.position;
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
    override
    public void TakeDamage(int damage){
		this.health -= damage;
	}

    IEnumerator hideTracer(float delay)
    {
        yield return new WaitForSeconds(delay);
        tracer.enabled = false;
    }

    //fire the gun
    override
    public void FireWeapon()
    {
        if(weaponTransform != null)
        {
            if (RefireDelay <= 0)
            {
                RefireDelay = Refire;

                Transform barrel = null;
                int barrelValue = Random.Range(0, barrels.Count);
                if (barrels.Count > 0){
                    barrel = barrels[barrelValue];
                }
                else
                {
                    Debug.Log("Enemy has no barrels assigned.");
                }
                RaycastHit hitInfo;
                if (barrel != null && Physics.Raycast(barrel.position, barrel.forward, out hitInfo, 1000, gunLayerMask) && this.ammo>0)
                {
                    tracer.SetPositions(new Vector3[] { barrel.position, hitInfo.point });
                    tracer.material.mainTextureOffset = new Vector2(-Random.value, 0);
                    tracer.enabled = true;

                    StopAllCoroutines();
                    IEnumerator coroutine = hideTracer(0.1f);
                    StartCoroutine(coroutine);
                    
                    // TODO sounds + light

                    Rigidbody targetRB = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                    HealthBar targetHealth = hitInfo.transform.root.GetComponentInChildren<HealthBar>();
                    PlayerHealth playerHealth = hitInfo.transform.root.GetComponentInChildren<PlayerHealth>();


                    if (targetHealth != null)
                        targetHealth.TakeDamage(weaponDamage);

                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(weaponDamage);
                        Debug.Log(playerHealth.currentHealth);
                    }
                    this.ammo--;

                    if (barrelValue == 0)
                    {
                        source.PlayOneShot(leftBarrelNoise, Random.Range(0.25f, 0.35f));
                    }
                    else
                    {
                        source.PlayOneShot(rightBarrelNoise, Random.Range(0.25f, 0.35f));
                    }
                }
                

                /* TODO
                GameObject bulletGO = Instantiate(bullet, this.transform.position, this.transform.rotation);
                bulletGO.GetComponent<BulletDamage>().ignore.Add(this.gameObject);
                bulletGO.GetComponent<Rigidbody>();
                ammo--;
                */
            }
            else if (RefireDelay > 0)
                RefireDelay -= Time.deltaTime;

            if (this.ammo == 0)
            {
                if (reloadNoiseActive)
                {
                    source.PlayOneShot(reloadNoise, Random.Range(0.5f, 0.75f));
                    reloadNoiseActive = false;
                }
                //die();
                Reload();
                //DeactivateShield();
                
            }
            

        }
    }

    public void Reload()
    {
        //this.die();
        if(this.reloadTime <= 0.0f)
        {
            //ActivateShield();
            this.ammo = 20;
            reloadTime = 3.0f;
            reloadNoiseActive = true;
        }
        else
        {
            reloadTime -= Time.deltaTime;
            
        }

    }


    public void DeactivateShield()
    {
        
        if (this.timeForDeactivation>1.5f)
        {
            this.timeForDeactivation -= Time.deltaTime;
            float time = 2.0f - (timeForDeactivation);
            float newScale = shieldDeactivation.Evaluate(time);
            //Debug.Log(time);
            Vector3 newScaleVector = new Vector3(newScale, newScale, newScale);
            shield.transform.localScale = this.defaultShield.localScale * newScale;
           // Vector3.Scale
        }
        else
        {
            this.shield.SetActive(false);
            //this.die();
            
        }
    }

    public void ActivateShield()
    {
        //TODO add animation
        Debug.Log("Shied activated");
        this.shield.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        this.shield.SetActive(true);      
    }


    

}
