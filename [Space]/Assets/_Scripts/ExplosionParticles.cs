/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticles : MonoBehaviour
{

    // Rate at which the used lights decay
    public float lightDecayRate = 0.99f;

    // Particle Systems used by the effect
    public List<ParticleSystem> particleSystems;
    // Lights used by the effect
    public List<Light> lights;
    // Copy of the initial light intensities
    private float[] maxIntensity;
    // If this is true then the effect will destroy itself when it ends
    public bool destroyOnFinish = false;

    // Keep track of whether the effect is running
    private bool isRunning = false;

    //explosion noise
    public AudioClip explosionNoise;
    public AudioSource source;
    float vol;

    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < particleSystems.Count; ++i)
        {
            particleSystems[i].Stop();
            particleSystems[i].gameObject.SetActive(false);
        }
        maxIntensity = new float[lights.Count];
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].gameObject.SetActive(false);
            lights[i].enabled = false;
            maxIntensity[i] = lights[i].intensity;
        }
    }

    // Start the effect
    public void play()
    {
        vol = Random.Range(0.5f, 0.75f);
        source.PlayOneShot(explosionNoise, vol);

        for (int i = 0; i < particleSystems.Count; ++i)
        {
            particleSystems[i].gameObject.SetActive(true);
            particleSystems[i].Play();
        }

        for (int i = 0; i < lights.Count; i++)
        {
            //          lights[i].intensity = maxIntensity[i];
            lights[i].gameObject.SetActive(true);
            lights[i].enabled = true;
        }

        isRunning = true;
    }

    // Set the effect to destroy after it has finished
    public void destroyAfterCompletion()
    {
        destroyOnFinish = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If the effect is running then fade the lights and check if still running
        if (isRunning)
        {

            // Decrement the light intensities
            for (int i = 0; i < lights.Count; i++)
            {
				lights[i].intensity *= lightDecayRate;
            }
            // Check if the particle systems are still going
            bool stillActive = false;
            for (int i = 0; i < particleSystems.Count; ++i)
            {
                if (particleSystems[i].IsAlive())
                    stillActive = true;
            }

            // If everything has finished then stop
            if (!stillActive)
            {
                for (int i = 0; i < lights.Count; i++)
                {
                    lights[i].gameObject.SetActive(false);
                    lights[i].enabled = false;
                }

                isRunning = false;
                // Destroy if set to
                if (destroyOnFinish)
                {
                    Destroy(this.gameObject);
                }

            }

        }

    }
}
