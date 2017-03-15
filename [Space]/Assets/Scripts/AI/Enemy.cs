using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using space;

//[RequireComponent (typeof (SphereCollider)), RequireComponent (typeof (Detection))]
[RequireComponent (typeof (LineRenderer))]
public class Enemy : GameEnemy {

    public List<Transform> barrels;
    private LineRenderer tracer;
    public LayerMask gunLayerMask;
    public float weaponDamage = 10.0f;

    override
    public void Start()
    {
        base.Start();

        tracer = GetComponent<LineRenderer>();
        tracer.numPositions = 2;
        tracer.enabled = false;
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
                if(barrels.Count > 0){
                    barrel = barrels[Random.Range(0, barrels.Count)];
                }
                else
                {
                    Debug.Log("Enemy has no barrels assigned.");
                }
                RaycastHit hitInfo;
                if (barrel != null && Physics.Raycast(barrel.position, barrel.forward, out hitInfo, 1000, gunLayerMask))
                {
                    Debug.Log(hitInfo.collider.gameObject.name);
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
        }
    }

}
