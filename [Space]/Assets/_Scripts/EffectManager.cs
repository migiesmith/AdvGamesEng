/// ----------------------------------------
/// Author: Grant Smith (40111906 / migiesmith)
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    public GameObject effectObject;
	public float lifeTime = 5.0f;
	[Space]
	public int maxInstances = 4;
	public float spawnDelay = 0.5f;

	float lastSpawn;

    public class EffectInstance
    {
        public GameObject instance;
        public float lifeTime;

		public EffectInstance(GameObject effect, float lifeTime, Vector3 position)
		{
			instance = Instantiate(effect, position, new Quaternion());
			this.lifeTime = lifeTime;
		}
		public void destroy(){Destroy(instance);}
    }

    List<EffectInstance> effectInstances = new List<EffectInstance>();

    // Use this for initialization
    void Start()
    {
        lastSpawn = Time.time;
    }

    void Update()
    {
        List<EffectInstance> toRemove = new List<EffectInstance>();
        for (int i = 0; i < effectInstances.Count; ++i)
        {
            effectInstances[i].lifeTime -= Time.deltaTime;
            if (effectInstances[i].lifeTime <= 0.0f)
            {
                effectInstances[i].destroy();
                toRemove.Add(effectInstances[i]);
            }
        }

        for (int i = 0; i < toRemove.Count; ++i)
        {
            effectInstances.Remove(toRemove[i]);
        }

    }

	protected void destroyInstance(EffectInstance instance)
	{
		effectInstances.Remove(instance);
		instance.destroy();
	}

    public void spawnSmoke(Vector3 position)
    {
		if(Time.time - lastSpawn < spawnDelay)
			return;

		if(effectInstances.Count >= maxInstances)
		{
			EffectInstance lowest = null;
        	for (int i = 0; i < effectInstances.Count; ++i)
			{
				if(lowest == null || effectInstances[i].lifeTime < lowest.lifeTime)
					lowest = effectInstances[i];
			}
			destroyInstance(lowest);
		}
		//Spawn Effect
		effectInstances.Add(new EffectInstance(effectObject, lifeTime, position));

		lastSpawn = Time.time;
    }

}
