/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections.Generic;
using UnityEngine;
using space;

public class VoidShot : MonoBehaviour
{
    
    [Space] public float effectDuration = 1.0f;
    public int damage = 10;

    [Header("Prime")]
    public Scale primeEffect;
    // Proportion of the duration that is for charging up / priming
    [Range(0.0f, 1.0f)] public float primeRatio = 0.9f;


    [Header("Explosion")]

    public GameObject explosion;
    // Duration before explosion is disabled after being activated
    public float explosionDuration = 1.0f;
    // Range of the explosion effect
    public float suctionRange = 1.0f;
    // Pulling force of the explosion
    public float suctionPower = 500.0f;
    // Duration of the explosion portion of the effect
    private float timeToExplode;
    // Duration left until explosion is finished
    private float timeTillExplosionOver;


    [Header("Audio")]
    public AudioSource audioSource;
    public float audioDurationScale = 1.0f;

    [Header("Other")]

    public LayerMask layerMask;
    public List<string> ignoreTags;

    public bool destroyOnExploded = true;
    public bool autoTrigger = true;

    private NewtonVR.NVRPlayer player;


    // Use this for initialization
    void Start()
    {
        // Prepare the objects
        primeEffect.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        primeEffect.scaleUpEnd.AddListener(primed);
        primeEffect.scaleDownEnd.AddListener(explode);
        explosion.SetActive(false);

        player = FindObjectOfType<NewtonVR.NVRPlayer>();

        // Trigger if set to auto trigger
        if (autoTrigger)
            trigger();
    }

    protected bool isPlayerHolding(GameObject go)
    {
        // Check each hand to see if the object is being held
        foreach (NewtonVR.NVRHand hand in player.Hands)
        {
            if (hand != null && hand.CurrentlyInteracting != null)
            {
                if (hand.CurrentlyInteracting.gameObject == go || go.transform.IsChildOf(hand.CurrentlyInteracting.transform))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void explode()
    {
        explosion.SetActive(true);
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            Vector3 dir = (rb.transform.position - this.transform.position);

            // Skip if the rb is outside of the range
            if (dir.magnitude > this.suctionRange)
                continue;

            // Don't effect held objects
            if (isPlayerHolding(rb.gameObject))
                continue;

            // Check if the Rigidbody is in line of sight
            RaycastHit hitInfo;
            if (rb.GetComponent<ShieldController>() == null && Physics.Raycast(this.transform.position, (dir).normalized, out hitInfo, suctionRange, layerMask))
            {
                // Check if there was an obstacle, if not, check if the tag is allowed
                if (hitInfo.collider.GetComponent<Rigidbody>() == rb && !ignoreTags.Contains(rb.tag))
                {
                    Enemy enemy = rb.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        HealthBar enemyHP = enemy.GetComponent<HealthBar>();
                        if(enemyHP != null)
                            enemyHP.TakeDamage(damage);
                        // Set the enemy to return to a location once recovered
                        enemy.ToAlert();
                        enemy.lastKnownLocation = enemy.transform.position;
                    }
                    // Set the temporary state of the Rigidbody
                    TemporaryRigidbodyState rbState = rb.gameObject.AddComponent<TemporaryRigidbodyState>();
                    rbState.isKinematic = false;
                    rbState.duration = 2.0f;
                    rbState.set();

                    // Apply suction force
                    rb.AddExplosionForce(dir.magnitude * -suctionPower, this.transform.position, suctionRange, 0.0f, ForceMode.Acceleration);
                    if (destroyOnExploded)
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }

    public void trigger()
    {
        timeToExplode = effectDuration;
        primeEffect.scaleDuration = effectDuration * primeRatio;
        primeEffect.setScale(1.0f);
        if (audioSource != null)
        {
            audioSource.pitch = audioDurationScale * audioSource.clip.length / (effectDuration);
            audioSource.Play();
        }
    }

    public void primed()
    {
        primeEffect.scaleDuration = effectDuration * (1.0f - primeRatio);
        primeEffect.setScale(0.01f);
        timeTillExplosionOver = explosionDuration;
        explosion.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToExplode > 0.0f)
        {
            timeToExplode -= Time.deltaTime;
            if (timeToExplode <= 0.0f)
            {
                explode();
            }
        }
        else if (timeTillExplosionOver > 0.0f)
        {
            timeTillExplosionOver -= Time.deltaTime;
            if (timeTillExplosionOver <= 0.0f)
            {
                explosion.SetActive(false);
            }
        }
    }
}
