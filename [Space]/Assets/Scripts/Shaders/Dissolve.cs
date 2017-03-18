/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{

    public float lifeTime = 1.0f;
    [HideInInspector] public float lifeLeft = 0.0f;
    public Gradient colorOverLife;
	public Texture dissolveTex;
    private Material[] dissolveMats;
    private List<Material[]> materials;

    // Use this for initialization
    void Start()
    {
        lifeLeft = lifeTime;
    }

	void Update()
	{
		if(Input.GetKey(KeyCode.A))
		{
			dissolveIn();
		}
		if(Input.GetKey(KeyCode.D))
		{
			dissolveOut();
		}
	}

    void dissolveIn()
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
					newMats[j] = new Material(Shader.Find("Space/Dissolve"));
					newMats[j].CopyPropertiesFromMaterial(renderers[i].materials[j]);
					newMats[j].SetTexture("_DissolveTex", dissolveTex);
					newMats[j].SetFloat("_Mode", 2.0f);
					newMats[j].EnableKeyword("_ALPHATEST_ON");
					newMats[j].EnableKeyword("_ALPHABLEND_ON");
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
