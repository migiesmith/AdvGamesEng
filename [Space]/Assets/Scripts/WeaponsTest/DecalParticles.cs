using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalParticles : MonoBehaviour {
    private ParticleSystem decal;
    private ParticleSystem.EmitParams emitParams;

    private void Start()
    {
        decal = GetComponent<ParticleSystem>();
    }

    public void spawnDecal(Vector3 position, Vector3 normal)
    {
        emitParams.rotation3D = Quaternion.LookRotation(normal).eulerAngles;
        transform.position = position;
        decal.Emit(emitParams, 1);
    }
}
