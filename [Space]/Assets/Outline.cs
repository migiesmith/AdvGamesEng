/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Outline : MonoBehaviour
{

    private List<Material[]> materials;

    [Range(0.0f, 0.1f)]
    public float outlineSize = 0.03f;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            show();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            hide();
        }
    }

    void show()
    {
        if (materials == null)
        {

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            if (materials == null)
            {
                materials = new List<Material[]>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    materials.Add(renderers[i].materials);
                    Material[] newMats = new Material[materials[i].Length];
                    for (int j = 0; j < renderers[i].materials.Length; j++)
                    {
                        Material m = new Material(Shader.Find("Space/Outline"));
                        m.CopyPropertiesFromMaterial(renderers[i].materials[j]);
						m.SetFloat("_Outline", outlineSize);
                        newMats[j] = m;
                    }
                    renderers[i].materials = newMats;
                }
            }
        }
    }

    void hide()
    {
        if (materials != null)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].materials = materials[i];
            }
            materials = null;
        }
    }

}
