using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
        // TODO: Get a coin
        // TODO: Coin SFX
    }
}
