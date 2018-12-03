using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    
    public AudioClip sound;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) {
            return;
        }

        Destroy(gameObject);
        // TODO: Get a coin
        AudioSource.PlayClipAtPoint(sound, this.transform.position);
    }
}
