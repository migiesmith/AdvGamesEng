using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSpawner : MonoBehaviour
{

    public List<GameObject> levelSelect;
    public int minNumOptions = 6;
    public int maxNumOptions = 15;

	[Range(0.0f, 100.0f)]
    public float maxDist = 2.0f;
	[Range(0.0f, 100.0f)]
    public float minDist = 1.0f;

    // Use this for initialization
    void Start()
    {
		int numOptions = Random.Range(minNumOptions, maxNumOptions);
        for (int i = 0; i < numOptions; ++i)
        {
			GameObject toSpawn = levelSelect[Random.Range(0, levelSelect.Count)];
            GameObject lvlOption = Instantiate(toSpawn);

			lvlOption.transform.parent = this.transform;
			float range = maxDist - minDist;
            lvlOption.transform.localPosition = new Vector3(Random.Range(-range, range), 0.0f, Random.Range(-range, range));
			lvlOption.transform.localPosition += lvlOption.transform.localPosition.normalized * minDist;
			lvlOption.transform.rotation = Quaternion.LookRotation(lvlOption.transform.localPosition.normalized);
			lvlOption.name = toSpawn.name;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
