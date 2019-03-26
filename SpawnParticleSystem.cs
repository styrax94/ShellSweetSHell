using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticleSystem : MonoBehaviour {

    public GameObject particleSystemHolder;
	
     public void SpawningParticleSystem(Vector3 position, Quaternion rotation)
    {
        Instantiate(particleSystemHolder, position, rotation);
    }
}
