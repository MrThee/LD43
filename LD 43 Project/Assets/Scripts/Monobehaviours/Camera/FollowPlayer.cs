using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform kFocus;

    public struct CameraConfig {
        /// <summary>
        /// Distance from the player to the camera along the direction the camera is facing.
        /// </summary>
        public float distanceToFocus;

        /// <summary>
        /// How fast it moves to the desired position.
        /// </summary>
        public float speediness;

        // How far above the player the camera should focus on.
        public float focusOffset;
    }

    public CameraConfig config = new CameraConfig
    {
        distanceToFocus = 15,
        speediness = 0.05f,
        focusOffset = 1.5f,
    };

	// Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (kFocus == null) {
            return;
        }
        Vector3 focusPosition = kFocus.position + config.focusOffset * kFocus.up;
        Vector3 desiredPosition = focusPosition - config.distanceToFocus * transform.forward;
        Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, config.speediness);
        transform.position = newPosition;
	}
}
