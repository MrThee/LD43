using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : AIController {
    
    private Rigidbody body;

    public float speed = 1f;
    private Vector3 velocity = new Vector3();

	// Use this for initialization
	protected override void Start(){
        base.Start();
        body = GetComponent<Rigidbody>();
    }
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 desiredVelocity = speed * direction;
        velocity = Vector3.Slerp(velocity, desiredVelocity, 0.1f);
        body.velocity = velocity;
	}

    private void OnTriggerEnter(Collider other)
    {
        PlayerController hitPlayer = other.gameObject.GetComponent<PlayerController>();
        if (hitPlayer) {
            hitPlayer.TakeDamage(1, hitPlayer.transform.position - transform.position);
        }
    }
}
