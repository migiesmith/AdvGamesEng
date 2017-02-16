using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scale : MonoBehaviour
{

    public float scaleDelay = 0.0f;
    public float scaleDuration = 0.5f;



    [System.Serializable]
    public class TriggerHandler : UnityEvent{}
    public TriggerHandler scaleUpStart;
    public TriggerHandler scaleDownStart;
    public TriggerHandler scaleUpEnd;
    public TriggerHandler scaleDownEnd;



    private Vector3 originalScale;

    struct ScaleArgs{
        public float scale;
        public float duration;

        public ScaleArgs(float scale, float duration){
            this.scale = scale;
            this.duration = duration;
        }
    }

    public void setScale(float scale)
    {
        this.setScale(scale, -1.0f);
    }
    public void setScale(float scale, float duration)
    {
        this.scale(scale / this.transform.localScale.x, duration);
    }


    public void scale(float scale)
    {
        this.scale(scale, -1.0f);
    }
    public void scale(float scale, float duration)
    {
        float d = (duration < 0.0f ? this.scaleDuration : duration);
        StopAllCoroutines();
        StartCoroutine("scaleTransform", new ScaleArgs(scale, d));
    }

    IEnumerator scaleTransform(ScaleArgs args)
    {
        yield return new WaitForSeconds(scaleDelay); 

        if(args.scale < 1.0f){
            scaleDownStart.Invoke();
        }else{
            scaleUpStart.Invoke();
        }

        bool scaleUp = (args.scale >= 1.0f);

        originalScale = this.transform.localScale;
        Vector3 endScale = originalScale * args.scale;

        float currTime = 0.0f;

        while (currTime <= 1.0f)
        {
            currTime += Time.deltaTime / args.duration;
            this.transform.localScale = Vector3.Lerp(originalScale, endScale, currTime);
            yield return null;
        }


        this.transform.localScale = originalScale * args.scale;

        if(args.scale < 1.0f){
            scaleDownEnd.Invoke();
        }else{
            scaleUpEnd.Invoke();
        }
    }

}
