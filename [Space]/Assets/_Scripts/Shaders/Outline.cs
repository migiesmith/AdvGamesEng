/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Outline : MonoBehaviour
{

    private GameObject currentlyOutlined = null;
    private List<Material[]> materials;

    [Range(0.0f, 0.1f)]
    public float outlineSize = 0.01f;

    public void show(GameObject toOutline)
    {
        if(toOutline == null)
            return;

        if(toOutline != this.currentlyOutlined)
            hide(this.currentlyOutlined);

        if (materials == null)
        {

            Renderer[] renderers = toOutline.GetComponentsInChildren<Renderer>();
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
                        m.SetColor("_OutlineColor", Color.cyan);
                        newMats[j] = m;
                    }
                    renderers[i].materials = newMats;
                }
            }
        }
        this.currentlyOutlined = toOutline;
    }

    public void hide(GameObject toStopOutlining)
    {
        if(toStopOutlining == null)
            return;
        if(toStopOutlining != this.currentlyOutlined)
            return;

        if (materials != null)
        {
            Renderer[] renderers = toStopOutlining.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].materials = materials[i];
            }
            materials = null;
        }
    }

}
