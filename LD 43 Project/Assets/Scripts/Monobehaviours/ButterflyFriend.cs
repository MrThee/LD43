using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyFriend : MonoBehaviour
{

    private Rigidbody body;
    private PlayerController player;

    public float speed = 1f;
    public float turnSpeed = 0.1f;
    private Vector3 velocity = new Vector3();

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = (player.transform.position + 3f * player.transform.up) - transform.position;
        Vector3 desiredVelocity = Vector3.zero;

        if (distance.magnitude > 3f) {
            Vector3 direction = distance.normalized;
            desiredVelocity = speed * direction;
        }
        velocity = Vector3.Lerp(velocity, desiredVelocity, turnSpeed);
        body.velocity = velocity;
    }
}
