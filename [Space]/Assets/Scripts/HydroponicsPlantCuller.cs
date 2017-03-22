/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class HydroponicsPlantCuller : MonoBehaviour
{

	private bool isActive = false;

    // Use this for initialization
    void Start()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        SphereCollider sc = this.GetComponent<SphereCollider>();
        sc.isTrigger = true;

		toggleChildren(isActive);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void toggleChildren(bool isActive)
    {

        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }

    void OnTriggerEnter(Collider other)
    {	
        if (other.tag.Equals("PlayerSensor"))
        {
            toggleChildren(isActive = true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("PlayerSensor"))
        {
            toggleChildren(isActive = false);
        }
    }
}
