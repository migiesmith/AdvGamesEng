/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (ParticleSystem))]
public class ParticleSparks : MonoBehaviour
{

	// The number of lights to use (more = better visuals, less = better performance)
    public int lightCount = 3;

	[Range(0,1)]
	// The rate at which the lights decay when not attached to an active particle (light.intensity *= lightDecay)
	public float lightDecay = 0.65f;
	// Whether to use the colour of the particles or the lightTemplate
	public bool useParticleColourForLight = true;
	// The object that will be created lightCount times and used as the light source
    public GameObject lightTemplate;
	// The list of created lights
    private GameObject[] lights;
	// The particle system attached to this GameObject
	private ParticleSystem partSys;
	// The intensity of the lightTemplate
	private float initalIntensity = 1.0f;

    // Use this for initialization
    void Start()
    {
		// Get the particle system
		this.partSys = this.GetComponent<ParticleSystem>();

		// Create the lights needed
		createLights(lightCount);

		Light lightComp = lightTemplate.GetComponent<Light>();
		this.initalIntensity = lightComp.intensity;
    }

	// Create the specificed number of lights for lighting the particles
	void createLights(int count){
		lights = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
			lights[i] = GameObject.Instantiate(lightTemplate);
			lights[i].transform.parent = this.transform;
			lights[i].transform.localPosition = new Vector3(0,0,0);
			lights[i].SetActive(false);
        }
	}

    // Update is called once per frame
    void Update()
    {
		// Create an array to store the particles
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[partSys.main.maxParticles];
		// Fill the particles list
		int partCount = partSys.GetParticles(particles);
		if(particles != null)
		{	
			// Loop through each available light
			foreach(GameObject l in lights)
			{
				// Update the lights using a random active particle
				Light lightComp = l.GetComponent<Light>();
				int index = Random.Range(0, partCount);
				if(particles[index].remainingLifetime > 0)
				{
					l.SetActive(true);
					l.transform.localPosition = particles[index].position;
					lightComp.intensity = Random.Range(0.0f, initalIntensity);
					// Set the colour of the light
					if(useParticleColourForLight)
						lightComp.color = particles[index].GetCurrentColor(partSys);
				}
				else
				{
					// Apply the decay so that lights fade when stationary
					lightComp.intensity *= lightDecay;
				}
			}
		}
    }
}
