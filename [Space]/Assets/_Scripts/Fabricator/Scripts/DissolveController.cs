using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class DissolveController : MonoBehaviour
    {
        private Dissolve dissolve;
 
        public void setAndDissolveIn(Gradient dGrad, Texture dTex)
        {
            dissolve = gameObject.AddComponent<Dissolve>();
            dissolve.colorOverLife = dGrad;
            dissolve.dissolveTex = dTex;
            dissolve.dissolveIn();
        }

        public void setAndDissolveOut(Gradient dGrad, Texture dTex)
        {
            dissolve = gameObject.AddComponent<Dissolve>();
            dissolve.colorOverLife = dGrad;
            dissolve.dissolveTex = dTex;
            dissolve.dissolveOut();
        }

        public void dissolveInComplete()
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

        public void dissolveOutComplete()
        {
            Destroy(transform.root.gameObject);
        }
    }
}
