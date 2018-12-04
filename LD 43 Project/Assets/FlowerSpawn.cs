using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawn : MonoBehaviour {

    public Transform shut;
    public Transform open;

	// Use this for initialization
	void Awake () {
        Shut();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Open() {
        shut.gameObject.SetActive(false);
        open.gameObject.SetActive(true);
    }

    void Shut() {
        shut.gameObject.SetActive(true);
        open.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController == null)
        {
            return;
        }

        // Hide everything else
        FlowerSpawn[] spawns = FindObjectsOfType<FlowerSpawn>();
        foreach (FlowerSpawn spawn in spawns) {
            spawn.Shut();
        }

        Open();
        playerController.m_spawnPosition = transform.position + 1 * Vector3.up;
    }
}
