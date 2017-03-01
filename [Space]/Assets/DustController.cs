using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (ParticleSystem))]
public class DustController : MonoBehaviour {

	private ParticleSystem partSys;

	// Use this for initialization
	void Start () {
		partSys = this.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[partSys.maxParticles];
		int partCount = partSys.GetParticles(particles);
		for(int i = 0; i < partCount; i++)
		{
			particles[i].position = particles[i].position + new Vector3(0.1f, 0, 0);
		}
	}
}
