using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space {
    [RequireComponent(typeof(SceneLoadTest))]
    public class AirlockControl : MonoBehaviour {

        public bool missionComplete;
        private SceneLoadTest loader;
        public DoorSlider slider;
        // Use this for initialization
        void Start() {
            missionComplete = false;
            loader = GetComponent<SceneLoadTest>();
        }

        void playerInAirlock(Collider other)
        {
            if (other.GetComponentInParent<NVRHead>() != null)
            {
                if (missionComplete && (slider.getState() == DoorSlider.DoorState.OPENING||slider.getState() == DoorSlider.DoorState.OPEN))
                {
                    slider.close();
                    loader.loadScene();
                }
                else if (!missionComplete && (slider.getState() == DoorSlider.DoorState.CLOSING || slider.getState() == DoorSlider.DoorState.CLOSED))
                    slider.open();
            }
        }

        public void openAirlock()
        {
            missionComplete = true;
            slider.open();
        }

        public void closeAirlock()
        {
            missionComplete = false;
            slider.close();
        }

        void OnTriggerEnter(Collider other)
        {
            playerInAirlock(other);
        }

        void OnTriggerStay(Collider other)
        {
            playerInAirlock(other);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<NVRHead>() != null)
            {
                if (!missionComplete && (slider.getState() == DoorSlider.DoorState.OPENING || slider.getState() == DoorSlider.DoorState.OPEN))
                    slider.close();
            }
        }
    }
}
