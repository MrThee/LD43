using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public CharacterController kController;
	public Animation kAnimation;

	public MovementState movementState;

	// Use this for initialization
	void Start () {
		this.movementState = new MovementState();
	}
	
	// Update is called once per frame
	public void UpdateState(float deltaTime) {
		movementState.MaybeApplyGravity(deltaTime);
		Vector3 totalVelocity = movementState.CalcTotalVelocity();
		Vector3 moveDelta = totalVelocity * deltaTime;
		kController.Move(moveDelta);
	}

	void OnControllerColliderHit(ControllerColliderHit hitInfo) {
		if(hitInfo.normal.y > 0.707f) {
			movementState.LandVertical();
		}
	}
}
