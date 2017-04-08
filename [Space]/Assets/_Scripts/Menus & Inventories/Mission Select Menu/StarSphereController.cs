using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSphereController : MonoBehaviour {

	public float radius = 3.0f;


	private ParticleSystem partSys;
	private bool hasStarted = false;


	// Use this for initialization
	void Start () {			
			// Get the Particle System attatched to this
			partSys = this.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
		var emitParams = new ParticleSystem.EmitParams();
		emitParams.startLifetime = Mathf.Infinity;
		partSys.Emit(emitParams, partSys.main.maxParticles);

		if(!hasStarted){
			int numParticles = partSys.main.maxParticles;
			
			// Create a list to store the particles
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[numParticles];
			
			partSys.GetParticles (particles);

			for(int i = 0; i < numParticles; i++) {
				ParticleSystem.Particle p = particles[i];
				p.position = Random.insideUnitSphere * radius;
				p.startColor = new Color(1, 1, 1, 0.3f + p.position.magnitude * 0.7f);
				float s = Random.Range(0.01f, 0.05f);
				p.startSize3D = new Vector3(s,s,s);
				particles[i] = p;
			}
			partSys.SetParticles(particles, numParticles);

			hasStarted = true;
		}else{

		}


	}

}
