/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticles : MonoBehaviour {
    int i = 0;

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

	// Use this for initialization
	void Start () {
		foreach(ParticleSystem sys in particleSystems)
		{
			sys.Stop();
			sys.gameObject.SetActive(false);
		}
		maxIntensity = new float[lights.Count];
		for(int i = 0; i < lights.Count; i++)
		{
            lights[i].gameObject.SetActive(false);
			lights[i].enabled = false;
			maxIntensity[i] = lights[i].intensity;
        }
	}

	// Start the effect
	public void play()
	{
        Debug.Log("play called");
		foreach(ParticleSystem sys in particleSystems)
		{
			sys.gameObject.SetActive(true);
			sys.Play();
		}

		for(int i = 0; i < lights.Count; i++)
		{
//          lights[i].intensity = maxIntensity[i];
			lights[i].gameObject.SetActive(true);
			lights[i].enabled = true;
		}

		isRunning = true;
	}

	// Set the effect to destroy after it has finished
	void destroyAfterCompletion(){
		destroyOnFinish = true;
	}

	// Update is called once per frame
	void Update () {
        // TOOD Debug remove this
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        //play();
        //}

        //elegant and majestic solution to explosion bug
        if (i < 3)
        {
            play();
            i ++;
        }
        else
        {
            destroyAfterCompletion();
        }
			
		

		// If the effect is running then fade the lights and check if still running
		if(isRunning)
		{
            
			// Decrement the light intensities
			foreach(Light light in lights)
				light.intensity *= lightDecayRate;

			// Check if the particle systems are still going
			bool stillActive = false;
			foreach(ParticleSystem sys in particleSystems)
			{
				if(sys.IsAlive())
					stillActive = true;
			}

			// If everything has finished then stop
			if(!stillActive)
			{
                for (int i = 0; i < lights.Count; i++)
				{
					lights[i].gameObject.SetActive(false);
					lights[i].enabled = false;
				}

				isRunning = false;
				// Destroy if set to
				if(destroyOnFinish)
				{
					Destroy(this.gameObject);
				}
                
            }
            
        }

	}
}
