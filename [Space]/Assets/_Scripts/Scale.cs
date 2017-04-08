/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
/// ----------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Scale : MonoBehaviour
{

    // Duration to wait before scaling
    public float scaleDelay = 0.0f;
    // Duration to scale over
    public float scaleDuration = 0.5f;



    [System.Serializable]
    public class TriggerHandler : UnityEvent { }
    // Invoked when a scale up begins
    public TriggerHandler scaleUpStart;
    // Invoked when a scale down begins
    public TriggerHandler scaleDownStart;
    // Invoked when a scale up ends
    public TriggerHandler scaleUpEnd;
    // Invoked when a scale down ends
    public TriggerHandler scaleDownEnd;

    // Copy of the scale before any scaling occurs
    private Vector3 originalScale;

    // Stores the parameters for the coroutine
    struct ScaleArgs
    {
        // The scaling factor
        public float scale;
        // Duration to scale for
        public float duration;

        // Constructor to set the scale and duration
        public ScaleArgs(float scale, float duration)
        {
            this.scale = scale;
            this.duration = duration;
        }
    }

    // Calculates scaling required to be scaled at the scale passed in
    public void setScale(float scale)
    {
        this.setScale(scale, -1.0f);
    }
    // Calculates scaling required to be scaled at the scale passed in
    public void setScale(float scale, float duration)
    {
        this.scale(scale / this.transform.localScale.x, duration);
    }

    // Scales relative to current scale
    public void scale(float scale)
    {
        this.scale(scale, -1.0f);
    }
    // Scales relative to current scale
    public void scale(float scale, float duration)
    {
        float d = (duration < 0.0f ? this.scaleDuration : duration);
        StopAllCoroutines();
        StartCoroutine("scaleTransform", new ScaleArgs(scale, d));
    }

    // Coroutine to scale an object over a duration
    IEnumerator scaleTransform(ScaleArgs args)
    {
        // Wait for the delay to pass
        yield return new WaitForSeconds(scaleDelay);

        if (args.scale < 1.0f)
        {
            // Invoke scale down begins
            scaleDownStart.Invoke();
        }
        else
        {
            // Invoke scale up begins
            scaleUpStart.Invoke();
        }

        // Determine if we are scaling up or down
        bool scaleUp = (args.scale >= 1.0f);

        // Copy the current scale
        originalScale = this.transform.localScale;
        // Calculate the final scale
        Vector3 endScale = originalScale * args.scale;

        // Keep track of the time passed
        float currTime = 0.0f;
        while (currTime <= 1.0f)
        {
            // Incrment time passed
            currTime += Time.deltaTime / args.duration;
            // Set the scale relative to the original and final
            this.transform.localScale = Vector3.Lerp(originalScale, endScale, currTime);
            yield return null;
        }

        // Set the localScale to the endScale (to get the exact scale incase their is a slight decimal error)
        this.transform.localScale = endScale;

        if (args.scale < 1.0f)
        {
            // Invoke scale down ends
            scaleDownEnd.Invoke();
        }
        else
        {
            // Invoke scale up ends
            scaleUpEnd.Invoke();
        }
    }

}
