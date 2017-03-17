using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour {

	public Vector2 offset = new Vector2(0.0f, 0.0f);
	public Vector2 scrollSpeed = new Vector2(0.0f, 0.0f);
	public float hitDecayRate = 0.99f;
	private Renderer rend;

	private static readonly int MAX_HITS = 16;

	public Vector4[] hits;

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

		for(int i = 0; i < MAX_HITS; i++)
		{
			hits[i].w *= hitDecayRate;
		}

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

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);
		addHit(other.transform.position);
	}

}
