using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class DecalController : MonoBehaviour
    {
        public bool beginControl = false;
        private bool isFading = false;
        public float preFade = 5.0f;
        public float fadeTime = 1.0f;
        private float fadeRate;
        private Material matToFade;

        void Start()
        {
            matToFade = this.GetComponent<MeshRenderer>().material;
            fadeRate = 1 / fadeTime;
        }

        void Update()
        {
            if (beginControl)
            {
                if (preFade > 0)
                    preFade -= Time.deltaTime;
                else
                {
                    float alpha = matToFade.color.a;
                    alpha -= fadeRate * Time.deltaTime;
                    if (alpha <= 0)
                        Destroy(this.gameObject);
                    else
                    {
                        Color newColour = new Color(matToFade.color.r, matToFade.color.g, matToFade.color.b, alpha);
                        matToFade.SetColor("_Color", newColour);
                    }
                }
            }
        }
    }
}
