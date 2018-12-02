using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainGenerator : MonoBehaviour {

    public Transform[] mountains;
    public Vector3 spawnRange;
    public int numMountains;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < numMountains; i++)
        {
            Transform mountainType = mountains[Random.Range(0, mountains.Length)];

            Vector3 pos = new Vector3();
            pos.x = Random.Range(-spawnRange.x, spawnRange.x);
            pos.y = Random.Range(-spawnRange.y, spawnRange.y);
            pos.z = Random.Range(-spawnRange.z, spawnRange.z);

            Transform mountain = Instantiate(mountainType, transform.position + pos, Quaternion.identity);
        }
		
	}
	
}
