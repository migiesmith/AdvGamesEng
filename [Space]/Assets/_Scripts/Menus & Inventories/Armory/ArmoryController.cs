using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

namespace space {
    public class ArmoryController : MonoBehaviour {

        public Transform spawnLocation;
        public Gradient dissolveGradient;
        public Texture dissolveTex;
        public List<ArmoryTile> buttons = new List<ArmoryTile>();
        public List<GameObject> armoryList = new List<GameObject>();
        public Dictionary<string, int> armoryInventory = new Dictionary<string, int>();
        private int numScreens;
        private int screenIndex;

        // Use this for initialization
        void Start() {
            foreach (GameObject g in armoryList)
                armoryInventory.Add(g.name, 1);

            if (armoryList.Count % 3 == 0)
                numScreens = (armoryList.Count / 3) - 3;
            else
                numScreens = (armoryList.Count / 3) - 2;

            screenIndex = 0;
            initialiseDisplay();
            updateDisplay();
        }

        void initialiseDisplay()
        {
            foreach (ArmoryTile tile in buttons)
                tile.initialise();
        }

        public void updateDisplay()
        {
            int tileIndex = 0;
            for (int itemIndex = screenIndex * 3; itemIndex < armoryList.Count; ++itemIndex)
            {
                GameObject currItem = armoryList[itemIndex];
                if (armoryInventory[currItem.name] > 0)
                {
                    buttons[tileIndex].setItem(armoryList[itemIndex], armoryInventory[currItem.name]);
                    buttons[tileIndex].gameObject.SetActive(true);
                    ++tileIndex;
                    if (tileIndex >= buttons.Count)
                        break;
                }
            }

            if (tileIndex < buttons.Count)
            {
                for (int i = tileIndex; i < buttons.Count; ++i)
                    buttons[i].gameObject.SetActive(false);
            }
        }

        public void showDetails(GameObject item)
        {

        }

        public void spawnItem(GameObject item)
        {
            GameObject spawned = Instantiate(item, spawnLocation.position, spawnLocation.rotation);
            spawned.name = item.name;

            Rigidbody spawnedRB = spawned.GetComponent<Rigidbody>();
            if (spawnedRB != null)
            {
                spawnedRB.useGravity = false;
                spawnedRB.isKinematic = true;
            }

            NVRInteractable spawnedInt = spawned.GetComponent<NVRInteractable>();
            if (spawnedInt != null)
                spawnedInt.enabled = false;

            DissolveController dissolve = spawned.AddComponent<DissolveController>();
            dissolve.setAndDissolveIn(dissolveGradient, dissolveTex);

            --armoryInventory[item.name];
            updateDisplay();
        }

        public void displayPrevious()
        {
            --screenIndex;
            if (screenIndex < 0)
                screenIndex = numScreens;
            updateDisplay();
        }

        public void displayNext()
        {
            ++screenIndex;
            if (screenIndex > numScreens)
                screenIndex = 0;
            updateDisplay();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.tag.Equals("Weapon") && !other.isTrigger)
            {
                NVRInteractable otherInt = other.transform.root.GetComponent<NVRInteractable>();
                if (otherInt != null && !otherInt.IsAttached)
                {
                    if (armoryInventory.ContainsKey(other.transform.root.name))
                        ++armoryInventory[other.transform.root.name];
                    updateDisplay();
                    DissolveController dissolve = other.gameObject.AddComponent<DissolveController>();
                    dissolve.setAndDissolveOut(dissolveGradient, dissolveTex);
                    return;
                }
            }
        }
    }
}
