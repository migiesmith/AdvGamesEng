using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NewtonVR;

public class TeleportCollide : MonoBehaviour
{

    public float lifeTime = 2.5f;

    private int points = 10;

    public Transform toTeleport;
    private NVRHand[] hands;

    // Use this for initialization
    void Start()
    {
        hands = FindObjectsOfType<NVRHand>();
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
                foreach (NVRHand hand in hands)
                    if (hand.CurrentlyInteracting != null)
                        hand.CurrentlyInteracting.transform.parent = hand.transform;
                toTeleport.position = contact.point;
                foreach (NVRHand hand in hands)
                    if (hand.CurrentlyInteracting != null)
                        hand.CurrentlyInteracting.transform.parent = null;
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
                foreach (NVRHand hand in hands)
                    if (hand.CurrentlyInteracting != null)
                        hand.CurrentlyInteracting.transform.parent = hand.transform;
                toTeleport.position = contact.point;
                foreach (NVRHand hand in hands)
                    if (hand.CurrentlyInteracting != null)
                        hand.CurrentlyInteracting.transform.parent = null;
                Destroy(this.gameObject);
                break;
            }
        }
    }

}
