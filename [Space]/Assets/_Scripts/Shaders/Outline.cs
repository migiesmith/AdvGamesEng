/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    private GameObject currentlyOutlined = null;
    private Dictionary<Renderer, Material[]> materials;

    [Range(0.0f, 1.0f)]
    public float outlineSize = 0.02f;

    public void show(GameObject toOutline)
    {
        Renderer topRenderer = toOutline.GetComponent<Renderer>();
        if(topRenderer == null)
        {
            topRenderer = toOutline.GetComponentInChildren<Renderer>();
        }
        if(topRenderer == null)
            return;
        float newOutineSize = outlineSize * (topRenderer.bounds.size.magnitude * topRenderer.transform.localScale.magnitude);

        if (toOutline == null)
            return;

        if (toOutline != this.currentlyOutlined)
            hide(this.currentlyOutlined);

        if (materials == null)
        {
            Renderer[] renderers = toOutline.GetComponentsInChildren<Renderer>();
            if (materials == null)
            {
                materials = new Dictionary<Renderer, Material[]>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    materials.Add(renderers[i], renderers[i].materials);
                    Material[] newMats = new Material[renderers[i].materials.Length];
                    for (int j = 0; j < renderers[i].materials.Length; j++)
                    {
                        Material m = new Material(Shader.Find("Space/Outline"));
                        m.CopyPropertiesFromMaterial(renderers[i].materials[j]);
                        m.SetFloat("_Outline", newOutineSize);
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
        if (toStopOutlining == null)
            return;
        if (toStopOutlining != this.currentlyOutlined)
            return;

        if (materials != null)
        {
            foreach (KeyValuePair<Renderer, Material[]> keyVal in materials)
            {
                keyVal.Key.materials = keyVal.Value;
            }
            materials = null;
        }
    }

}
