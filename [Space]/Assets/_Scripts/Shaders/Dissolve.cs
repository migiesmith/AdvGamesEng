/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Dissolve : MonoBehaviour
{

    public float lifeTime = 1.0f;
    [HideInInspector] public float lifeLeft = 0.0f;
    public Gradient colorOverLife;
	[Header("Texture")]
	public Texture dissolveTex;
	
	public Vector2 scale = new Vector2(1,1);
	public Vector2 offset = new Vector2(0,0);
    private Material[] dissolveMats;
    private List<Material[]> materials;


    // Use this for initialization
    void Start()
    {
        lifeLeft = lifeTime;
    }

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			dissolveIn();
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			dissolveOut();
		}
	}

    public void dissolveIn()
    {
        StopAllCoroutines();
        StartCoroutine("dissolve", true);
    }

    void dissolveOut()
    {
        StopAllCoroutines();
        StartCoroutine("dissolve", false);
    }

    // Coroutine to dissolve an the obect over a duration
    IEnumerator dissolve(bool fadingIn)
    {

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (materials == null)
        {
			materials = new List<Material[]>();
            for (int i = 0; i < renderers.Length; i++)
            {
				materials.Add(renderers[i].materials);
				Material[] newMats = new Material[materials[i].Length];
				for(int j = 0; j < renderers[i].materials.Length; j++)
				{
					
					Material m = new Material(Shader.Find("Space/Dissolve"));
					m.CopyPropertiesFromMaterial(renderers[i].materials[j]);
					m.SetTexture("_DissolveTex", dissolveTex);
					m.SetTextureOffset("_DissolveTex", offset);
					m.SetTextureScale("_DissolveTex", scale);
					newMats[j] = m;
				}
				renderers[i].materials = newMats;
            }
        }

        // Enable all renderers
        foreach (Renderer rend in renderers)
        {
			if(!(rend is LineRenderer))
            	rend.enabled = true;
        }

		float alphaVal = fadingIn ? 0.0f : 1.0f;
		float dissolveSpeed = 1.0f / lifeTime * (fadingIn ? 1 : -1);
        while ((fadingIn && alphaVal < 1.0f) || (!fadingIn && alphaVal > 0.0f))
        {
            alphaVal += Time.deltaTime * dissolveSpeed;

            for (int i = 0; i < renderers.Length; i++)
            {
				for(int j = 0; j < renderers[i].materials.Length; j++)
				{
					Color newCol = colorOverLife.Evaluate(Mathf.Clamp(alphaVal, 0.0f, 1.0f));	
					///newCol.a = alphaVal;
					// Fade the colour of each renderer
					renderers[i].materials[j].color = newCol;
				}
            }

            yield return null;
        }

		if(fadingIn){
			// Return to original materials
			for (int i = 0; i < renderers.Length; i++)
			{
				renderers[i].materials = materials[i];
			}
			materials = null;
		}else{
			// Disable all renderers
			foreach (Renderer rend in renderers)
			{
				rend.enabled = false;
			}
		}
    }

}
