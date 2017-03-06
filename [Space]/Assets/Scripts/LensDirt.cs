using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LensDirt : MonoBehaviour {

	public Texture dirtTexture;
	[Range(0,1)]
	public float intensity = 1.0f;

	private Material material;


	// Use this for initialization
	void Awake () {
		material = new Material(Shader.Find("Hidden/LensDirt"));
	}
	
	// Update is called once per frame
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if(intensity == 0){
			Graphics.Blit(source, destination);
			return;
		}

		material.SetFloat("_fintensity", intensity);
		material.SetTexture("_DirtTex", dirtTexture);
		Graphics.Blit (source, destination, material);
	}
}
