using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour {
    
    public Rigidbody body;
    public PlayerController player;

    public float speed = 1f;
    private Vector3 velocity = new Vector3();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 desiredVelocity = speed * direction;
        velocity = Vector3.Slerp(velocity, desiredVelocity, 0.1f);
        body.velocity = velocity;
	}

    private void OnTriggerEnter(Collider other)
    {
        PlayerController hitPlayer = other.gameObject.GetComponent<PlayerController>();
        if (hitPlayer) {
            hitPlayer.TakeDamage(1, (transform.position - hitPlayer.transform.position).normalized);
        }
    }
}
