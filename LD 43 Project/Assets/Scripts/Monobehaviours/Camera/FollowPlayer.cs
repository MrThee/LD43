using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform kFocus;

    /// <summary>
    /// Distance from the player to the camera along the direction the camera is facing.
    /// </summary>
    public float distanceToFocus = 15;

    /// <summary>
    /// How fast it moves to the desired position.
    /// </summary>
    public float speediness = 0.1f;

    // How far above the player the camera should focus on.
    public float focusOffset = 3f;

	// Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 focusPosition = kFocus.position + focusOffset * kFocus.up;
        Vector3 desiredPosition = focusPosition - distanceToFocus * transform.forward;
        Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, 0.1f);
        transform.position = newPosition;
	}
}
