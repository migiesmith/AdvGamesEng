using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour{

    public GameObject shot;

    public float Refire = 0.2f;
    private float RefireDelay = 0.0f;
    public int ammo = 10;

    //fire weapon
    public void fire()
    {
        Debug.Log("Weapon would fire");
        if (RefireDelay <=0)
        {
            RefireDelay = Refire;
            Instantiate(shot, this.transform.position, this.transform.rotation);
            ammo--;
        }
        else if (RefireDelay > 0)
            RefireDelay -= Time.deltaTime;
    }
}
