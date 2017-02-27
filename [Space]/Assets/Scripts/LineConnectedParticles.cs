using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (ParticleSystem)), RequireComponent (typeof (LineRenderer))]
public class LineConnectedParticles : MonoBehaviour {

	private ParticleSystem partSys;
	private LineRenderer lineRend;

	// Use this for initialization
	void Start () {
		this.partSys = this.GetComponent<ParticleSystem>();
		this.lineRend = this.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		ParticleSystem.Particle[] particles = {};
		int partCount = partSys.GetParticles(particles);

		Vector3[] positions = new Vector3[partCount];
		for(int i = 0; i < partCount; i++){
			positions[i] = particles[i].position;
		}
		lineRend.numPositions = partCount;
		lineRend.SetPositions(positions);
	}
}
