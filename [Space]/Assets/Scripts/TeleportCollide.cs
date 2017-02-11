using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeleportCollide : MonoBehaviour
{

    public float lifeTime = 2.5f;

    private int points = 10;

    public Transform toTeleport;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
            Destroy(this.gameObject);

    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (collision.gameObject.tag.Equals("teleportable"))
            {
                toTeleport.position = contact.point;
                Destroy(this.gameObject);
                break;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (collision.gameObject.tag.Equals("teleportable"))
            {
                toTeleport.position = contact.point;
                Destroy(this.gameObject);
                break;
            }
        }
    }

}
