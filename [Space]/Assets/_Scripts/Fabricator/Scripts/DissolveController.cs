using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class DissolveController : MonoBehaviour
    {
        private Dissolve dissolve;
        public bool dissolveComplete;

        void Update()
        {
            if (dissolveComplete)
            {
                Rigidbody itemRB = transform.root.GetComponent<Rigidbody>();
                if (itemRB != null)
                {
                    itemRB.useGravity = true;
                    itemRB.isKinematic = false;
                }
                transform.root.GetComponent<NVRInteractable>().enabled = true;

                Destroy(dissolve);
                Destroy(this);
            }
        }

        public void setAndDissolve(Gradient dGrad, Texture dTex)
        {
            dissolveComplete = false;
            dissolve = gameObject.AddComponent<Dissolve>();
            dissolve.colorOverLife = dGrad;
            dissolve.dissolveTex = dTex;
            dissolve.dissolveIn();
        }
    }
}
