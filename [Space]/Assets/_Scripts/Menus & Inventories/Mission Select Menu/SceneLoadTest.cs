using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NewtonVR;

namespace space
{
    [RequireComponent(typeof(NVRInteractableItem)), RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
    public class SceneLoadTest : MonoBehaviour
    {
        private NVRPlayer player;
        private SteamVR_LoadLevel sceneLoader;
        // Use this for initialization
        void Start()
        {
            player = FindObjectOfType<NVRPlayer>();
            sceneLoader = GetComponent<SteamVR_LoadLevel>();
        }

        public virtual void loadScene()
        {
            DontDestroyOnLoad(player.gameObject);
            if (player.LeftHand.CurrentlyInteracting != null && player.LeftHand.CurrentlyInteracting.transform.root.gameObject != transform.root.gameObject)
                DontDestroyOnLoad(player.LeftHand.CurrentlyInteracting.transform.root.gameObject);
            if (player.RightHand.CurrentlyInteracting != null && player.RightHand.CurrentlyInteracting.transform.root.gameObject != transform.root.gameObject)
                DontDestroyOnLoad(player.RightHand.CurrentlyInteracting.transform.root.gameObject);
            sceneLoader.Trigger();
        }
    }
}
