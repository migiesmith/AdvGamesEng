/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Fade : MonoBehaviour
{
    // Duration to wait before fading
    public float fadeDelay = 0.0f;
    // Duration to fade in over
    public float fadeInDuration = 0.5f;
    // Duration to fade out over
    public float fadeOutDuration = 0.5f;
    // Whether or not to start faded
    public bool startFaded = false;

    [System.Serializable]
    public class TriggerHandler : UnityEvent { }
    // Invoked when a fade in begins
    public TriggerHandler fadeInStart;
    // Invoked when a fade out begins
    public TriggerHandler fadeOutStart;
    // Invoked when a fade in ends
    public TriggerHandler fadeInEnd;
    // Invoked when a fout out ends
    public TriggerHandler fadeOutEnd;

    // The colors of every renderer
    private Color[] colours;


    void Start()
    {
        // Start fading
        if (startFaded)
        {
            StartCoroutine("fade", 0.0f);
        }
    }

    // Fade in over the passed in duration (pass in a value less than 0 to use the default)
    public void fadeIn(float fadeDuration = -1.0f)
    {
        float duration = (fadeDuration < 0.0f ? fadeInDuration : fadeDuration);
        StopAllCoroutines();
        StartCoroutine("fade", duration);
    }

    // Fade out over the passed in duration (pass in a value less than 0 to use the default)
    public void fadeOut(float fadeDuration = -1.0f)
    {
        float duration = (fadeDuration < 0.0f ? fadeOutDuration : fadeDuration);
        StopAllCoroutines();
        StartCoroutine("fade", -duration);
    }

    // Coroutine that gradually fades the a hierarchy in or out
    IEnumerator fade(float fadeDuration)
    {
        // Wait for the fade delay
        yield return new WaitForSeconds(fadeDelay);

        // Determine if we are fading in or out
        bool fadingIn = (fadeDuration >= 0.0f);
        float fadeSpeed = 1.0f / fadeDuration;

        if (fadingIn)
        {
            // Invoke fade in begins
            fadeInStart.Invoke();
        }
        else
        {
            // Invoke fade out begins
            fadeOutStart.Invoke();
        }

        // Get all renderers to fade
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (colours == null)
        {
            colours = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                // Copy the colour of all renderers
                if (renderers[i].material.HasProperty("_Color"))
                {
                    colours[i] = renderers[i].material.color;
                }
                else if (renderers[i].material.HasProperty("_TintColor"))
                {
                    colours[i] = renderers[i].material.GetColor("_TintColor");
                }
            }
        }

        // Enable all renderers
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }

        float maxAlpha = getMaxAlpha();
        float alphaVal = maxAlpha;
        while ((fadingIn && alphaVal < 1.0f) || (!fadingIn && alphaVal > 0.0f))
        {
            alphaVal += Time.deltaTime * fadeSpeed;

            for (int i = 0; i < renderers.Length; i++)
            {
                // Calculate the new colour
                Color newCol = (colours != null ? colours[i] : renderers[i].material.color);
                newCol.a = Mathf.Clamp(Mathf.Min(newCol.a, alphaVal), 0.0f, 1.0f);
                // Fade the colour of each renderer
                if (renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.SetColor("_Color", newCol);
                }
            }

            yield return null;
        }

        if (!fadingIn)
        {
            // Disable all renderers
            foreach (Renderer rend in renderers)
            {
                rend.enabled = false;
            }
        }

        if (fadingIn)
        {
            // Invoke fade in ends
            fadeInEnd.Invoke();
        }
        else
        {
            // Invoke fade out ends
            fadeOutEnd.Invoke();
        }

    }

    private float getMaxAlpha()
    {
        float maxA = 0.0f;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            if (rend.material.HasProperty("_Color"))
            {
                if (maxA < rend.material.GetColor("_Color").a)
                {
                    maxA = rend.material.GetColor("_Color").a;
                }
            }
        }
        return maxA;
    }

}
