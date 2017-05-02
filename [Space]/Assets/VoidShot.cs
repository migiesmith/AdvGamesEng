using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidShot : MonoBehaviour
{

    public float explodeTime = 1.0f;
    [Range(0.0f, 1.0f)] public float primeRatio = 0.9f;
    private float timeToExplode;
    public float explosionDuration = 1.0f;
    private float timeTillExplosionOver;
    public float suctionRange = 1.0f;
    public float suctionPower = 500.0f;

	public LayerMask layerMask;
    public List<string> ignoreTags;

    public Scale prime;
    public GameObject explosion;

	public bool destroyOnExploded = true;
	public bool autoTrigger = true;

	public AudioSource audioSource;
	public float audioDurationScale = 1.0f;

    private NewtonVR.NVRPlayer player;


    // Use this for initialization
    void Start()
    {
        prime.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        prime.scaleUpEnd.AddListener(primed);
        prime.scaleDownEnd.AddListener(explode);
        explosion.SetActive(false);

		if(autoTrigger)
			trigger();

        player = FindObjectOfType<NewtonVR.NVRPlayer>();
    }

    protected bool isPlayerHolding(GameObject go)
    {
        foreach(NewtonVR.NVRHand hand in player.Hands)
        {
            if(hand.CurrentlyInteracting.gameObject == go || go.transform.IsChildOf(hand.CurrentlyInteracting.transform))
                return true;
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
            if(dir.magnitude > this.suctionRange)
                continue;

            if(isPlayerHolding(rb.gameObject))
                continue;

            RaycastHit hitInfo;
			if (rb.GetComponent<ShieldController>() == null && Physics.Raycast(this.transform.position, (dir).normalized, out hitInfo, 10, layerMask))
            {
                if (hitInfo.collider.GetComponent<Rigidbody>() == rb && !ignoreTags.Contains(rb.tag))
                {
					Enemy enemy = rb.GetComponent<Enemy>();
					if(enemy != null)
					{
						enemy.ToAlert();
						enemy.lastKnownLocation = enemy.transform.position;
					}
					TemporaryRigidbodyState rbState = rb.gameObject.AddComponent<TemporaryRigidbodyState>();
					rbState.isKinematic = false;
					rbState.duration = 2.0f;
					rbState.set();

                    // Apply suction force
                    rb.AddExplosionForce(dir.magnitude * -suctionPower, this.transform.position, suctionRange, 0.0f, ForceMode.Acceleration);
					if(destroyOnExploded)
					{
						Destroy(this.gameObject);
					}
                }
            }
        }
    }

    public void trigger()
    {
        timeToExplode = explodeTime;
        prime.scaleDuration = explodeTime * primeRatio;
        prime.setScale(1.0f);
		if(audioSource != null)
		{
			audioSource.pitch = audioDurationScale * audioSource.clip.length / (explodeTime);
			audioSource.Play();
		}
    }

    public void primed()
    {
        prime.scaleDuration = explodeTime * (1.0f - primeRatio);
        prime.setScale(0.01f);
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
