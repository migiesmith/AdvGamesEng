using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space
{
    public class PlayerToOrigin : MonoBehaviour
    {
        private GameObject player;
        public Vector3 origin;
        public enum StartFacing
        {
            Z_POSITIVE,
            X_POSITIVE,
            Z_NEGATIVE,
            X_NEGATIVE
        }
        public StartFacing rotation = StartFacing.Z_POSITIVE;
        // Use this for initialization
        void Start()
        {
            player = FindObjectOfType<NVRPlayer>().transform.root.gameObject;
            player.transform.position = origin;
            switch(rotation)
            {
                case StartFacing.Z_POSITIVE:
                    {
                        player.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                        break;
                    }
                case StartFacing.X_POSITIVE:
                    {
                        player.transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
                        break;
                    }
                case StartFacing.Z_NEGATIVE:
                    {
                        player.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
                        break;
                    }
                case StartFacing.X_NEGATIVE:
                    {
                        player.transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
                        break;
                    }
            }
        }
    }
}
