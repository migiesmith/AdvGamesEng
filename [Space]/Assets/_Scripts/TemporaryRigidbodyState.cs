using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryRigidbodyState : MonoBehaviour
{

    public float duration = 1.0f;
    private bool wasKinematic = true;
    public bool isKinematic = true;

    private bool wasUsingGravity = true;
    public bool useGravity = true;

    public RigidbodyConstraints constraints;
    private RigidbodyConstraints originalConstraints;

    private Rigidbody rb;

    private bool beenSet = false;

    // Use this for initialization
    void Start()
    {
        set();
    }

    public void set()
    {
        if (!beenSet)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null || GetComponents<TemporaryRigidbodyState>().Length > 1)
            {
                // This script can't be ran, destroy it
                Destroy(this);
            }
            else
            {
                // Copy the current state
                wasKinematic = rb.isKinematic;
                wasUsingGravity = rb.useGravity;
                originalConstraints = rb.constraints;
                // Set the temporary state
                rb.isKinematic = isKinematic;
                rb.useGravity = useGravity;
                rb.constraints = constraints;
                // Tell the system to destroy this script after a duration
                Destroy(this, duration);
                beenSet = true;
            }
        }
    }

    void OnDestroy()
    {
        if (beenSet)
        {
            // Restore the initial state
            rb.isKinematic = wasKinematic;
            rb.useGravity = wasUsingGravity;
            rb.constraints = originalConstraints;
        }
    }

}
