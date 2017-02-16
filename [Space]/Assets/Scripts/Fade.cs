using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fade : MonoBehaviour
{

    public float fadeDelay = 0.0f;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;
    public bool startFaded = false;

    [System.Serializable]
    public class TriggerHandler : UnityEvent{}
    public TriggerHandler fadeInStart;
    public TriggerHandler fadeOutStart;
    public TriggerHandler fadeInEnd;
    public TriggerHandler fadeOutEnd;

    private Color[] colors;


    void Start(){
        if(startFaded){
            StartCoroutine("fade", 0.0f);
        }
    }



    public void fadeIn(float fadeDuration = -1.0f)
    {
        float duration = (fadeDuration < 0.0f ? fadeInDuration : fadeDuration);
        StopAllCoroutines();
        StartCoroutine("fade", duration);
    }

    public void fadeOut(float fadeDuration = -1.0f)
    {
        float duration = (fadeDuration < 0.0f ? fadeOutDuration : fadeDuration);
        StopAllCoroutines();
        StartCoroutine("fade", -duration);
    }

    IEnumerator fade(float fadeDuration)
    {
        yield return new WaitForSeconds(fadeDelay);

        bool fadingIn = (fadeDuration >= 0.0f);
        float fadeSpeed = 1.0f / fadeDuration;

        if(fadingIn){
            fadeInStart.Invoke();
        }else{
            fadeOutStart.Invoke();
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (colors == null)
        {
            colors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_Color")){
                    colors[i] = renderers[i].material.color;
                }else if(renderers[i].material.HasProperty("_TintColor")){
                    colors[i] = renderers[i].material.GetColor("_TintColor");
                }
            }
        }

        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }

        float maxAlpha = getMaxAlpha();
        float alphaVal = maxAlpha;
        Debug.Log((fadingIn && alphaVal < 1.0f) +"||"+ (!fadingIn && alphaVal > 0.0f));
        while ((fadingIn && alphaVal < 1.0f) || (!fadingIn && alphaVal > 0.0f))
        {
            alphaVal += Time.deltaTime * fadeSpeed;

            for (int i = 0; i < renderers.Length; i++)
            {
                Color newCol = (colors != null ? colors[i] : renderers[i].material.color);
                newCol.a = Mathf.Clamp(Mathf.Min(newCol.a, alphaVal), 0.0f, 1.0f);
                if (renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.SetColor("_Color", newCol);
                }
                Debug.Log("fade");
            }

            yield return null;
        }


        if (!fadingIn)
        {
            foreach (Renderer rend in renderers)
            {
                rend.enabled = false;
            }
        }

        if(fadingIn){
            fadeInEnd.Invoke();
        }else{
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
