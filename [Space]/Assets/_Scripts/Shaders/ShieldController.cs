/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour {

	// Texture offset
	public Vector2 offset = new Vector2(0.0f, 0.0f);
	// Scroll speed of the textures
	public Vector2 scrollSpeed = new Vector2(0.0f, 0.0f);
	// Rate at which hits decay
	public float hitDecayRate = 0.99f;
	// Reference to this objects renderer
	private Renderer rend;

	// The particle systems used by the shield (used when breaking)
	public List<ParticleSystem> particleSystems;

	// Max hits allowed (used to match the shader)
	private static readonly int MAX_HITS = 16;

	// All hits (x,y,z = location, w = alpha)
	private Vector4[] hits;
	// If the shield is breaking
	private bool isBreaking = false;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		hits = new Vector4[MAX_HITS];
		for(int i = 0; i < MAX_HITS; i++)
		{
			hits[i] = new Vector4(0,0,0,0);
		}
	}
	
	// Update is called once per frame
	void Update () {

		// Check if the shield is breaking
		if(isBreaking){
			// Decrement the colour
			Color c = rend.material.GetColor("_Color");
			c.a *= 0.98f;
			rend.material.SetColor("_Color", c);

			// Start clipping if the colour is relatively transparent
			if(c.a < 0.25f)
				rend.material.SetFloat("_ClipAmount", (rend.material.GetFloat("_ClipAmount") + 0.001f) * 1.025f);
			
			// Randomise the hits
			for(int i = 0; i < MAX_HITS; i++)
			{
				if(Random.Range(0.0f, 1.0f) < 0.1f)
				{
					Vector3 pos = Vector3.Normalize(Random.insideUnitSphere);
					for(int j = 0; j < 3; j++)
						pos[j] *= transform.localScale[j] * 0.5f;
					pos += this.transform.position;
                    addHit(new Vector4(pos.x, pos.y, pos.z, Random.Range(0.0f, 0.75f)));
                }
			}

			bool stillRunning = false;
			// Stop all of the particle systems and see if they still have emitted particles
			foreach(ParticleSystem sys in particleSystems)
			{
				sys.Stop();
				if(sys.particleCount > 0)
					stillRunning = true;
			}
            // If all particles are gone, destroy this object
            /*if (!stillRunning)
                Destroy(this.gameObject); */ //Temporarily removed as a quick fix for error spam due to conflict with enemy scripts
		}

		// Decay all hits
		for(int i = 0; i < MAX_HITS; i++)
		{
			hits[i].w *= hitDecayRate;
		}

		// Update the shader
		if(rend != null){
			offset += scrollSpeed;
			
			rend.material.SetTextureOffset("_MainTex", offset);
			rend.material.SetTextureOffset("_ClipTex", offset);
			rend.material.SetTextureOffset("_DistortionTex", -offset);
			//rend.material.SetFloat("_Time", Time.time);
			
			var materialProperty = new MaterialPropertyBlock();
			materialProperty.SetVectorArray("_Hits", hits);
			gameObject.GetComponent<Renderer> ().SetPropertyBlock (materialProperty);
		}
	}

	// Add a new hit to the shield, removing the least visible
	public void addHit(Vector3 hit)
	{
		float lowest = hits[0].w;
		int lowestIdx = 0;
		for(int i = 1; i < MAX_HITS; i++){
			if(hits[i].w < lowest)
			{
				lowest = hits[i].w;
				lowestIdx = i;
			}
		}
		hits[lowestIdx] = new Vector4(hit.x, hit.y, hit.z, 1.0f);
	}

/*	// TODO remove this function once the guns use 'addHit'
	void OnTriggerEnter(Collider other)
	{
		addHit(other.transform.position);
	}*/

	public void breakShield()
	{
		isBreaking = true;
	}
}
