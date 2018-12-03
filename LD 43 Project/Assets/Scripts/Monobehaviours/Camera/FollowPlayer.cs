using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    private Transform m_focus;

    private PlayerController player;

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

        public enum Mode {
            Player,
            Transform
        }
        public Mode mode;
    }

    public CameraConfig config = new CameraConfig
    {
        distanceToFocus = 15,
        speediness = 0.05f,
        focusOffset = 1.5f,
        mode = CameraConfig.Mode.Player
    };

	// Use this for initialization
    void Start () {
        this.player = FindObjectOfType<PlayerController>();
	}

    public void Focus(Transform target){
        m_focus = target;
        config.mode = CameraConfig.Mode.Transform;
    }

    public void FocusOnPlayer(){
        config.mode = CameraConfig.Mode.Player;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        switch(config.mode){
            case CameraConfig.Mode.Transform:{
                Vector3 focusPosition = m_focus.position + config.focusOffset * m_focus.up;
                Vector3 desiredPosition = focusPosition - config.distanceToFocus * transform.forward;
                Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, config.speediness);
                transform.position = newPosition;
            }
            break;

            case CameraConfig.Mode.Player: {
                Vector3 focusPosition = player.transform.position + config.focusOffset * player.transform.up;
                Vector3 desiredPosition = focusPosition - config.distanceToFocus * transform.forward;
                float minOffset = 1;
                float pt = Mathf.InverseLerp(
                    0f, 
                    player.maxGroundSpeed,
                    player.kCharacter.movementState.lateralSpeed);
                float maxOffset = 5f;
                float offsetMag = Mathf.Lerp(minOffset, maxOffset, pt);
                Vector3 offset = player.kCharacter.kBodyTransform.forward * offsetMag;
                offset = Vector3.ProjectOnPlane(offset, Vector3.forward);
                desiredPosition += offset;
                transform.position = Vector3.Lerp(transform.position, desiredPosition, config.speediness);
            }
            break;
        }
	}
}
