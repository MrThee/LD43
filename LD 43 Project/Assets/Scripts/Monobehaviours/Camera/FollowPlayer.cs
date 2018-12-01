using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform kPlayer;

    /// <summary>
    /// Distance from the player to the camera along the direction the camera is facing.
    /// </summary>
    public float distanceToPlayer;

    /// <summary>
    /// How fast it moves to the desired position.
    /// </summary>
    public float speediness = 0.1f;

	// Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 desiredPosition = kPlayer.position - distanceToPlayer * transform.forward;
        Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, 0.1f);
        transform.position = newPosition;
	}
}
