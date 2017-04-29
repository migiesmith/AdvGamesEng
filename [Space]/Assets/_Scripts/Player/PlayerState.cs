using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space {
    public class PlayerState : MonoBehaviour {

        public List<GameObject> inCombat = new List<GameObject>();
        private bool isInMenu = false;
        public Dash2 dash;
        public Teleport[] teleport;
        public GameObject inventory;
        // Use this for initialization

        public void newThreat(GameObject threat)
        {
            inCombat.Add(threat);
        }

        public void threatOver(GameObject threat)
        {
            inCombat.Remove(threat);
        }

        public void leftScene()
        {
            inCombat.Clear();
        }

        public bool isInCombat()
        {
            if (inCombat.Count > 0)
                return true;
            else
                return false;
        }

        public void inMenu()
        {
            dash.enabled = false;
            foreach (Teleport tp in teleport)
                tp.enabled = false;
            inventory.SetActive(false);
            isInMenu = true;
        }

        public void leaveMenu()
        {
            if (isInMenu)
            {
                dash.enabled = true;
                foreach (Teleport tp in teleport)
                    tp.enabled = true;
                inventory.SetActive(true);
                isInMenu = false;
            }
        }
    }
}
