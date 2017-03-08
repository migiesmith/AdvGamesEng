using UnityEngine;
using System.Collections;

//[RequireComponent (typeof (SphereCollider)), RequireComponent (typeof (Detection))]
public class Enemy : GameEnemy {

    override
    public void Start()
    {
        base.Start();
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


    //fire the gun
    override
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
