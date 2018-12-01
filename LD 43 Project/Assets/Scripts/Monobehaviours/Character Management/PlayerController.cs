using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Character kCharacter;
	public float maxGroundSpeed = 10f;
	public float maxGroundDecelStartSpeed = 5f;
	public float fastFallSpeed = 16f;
	public float groundAcceleration = 80f;
	public float groundDeceleration = 120f;

	private InputParser mk_inputParser;
	// Use this for initialization.. TODO: delet this.
	void Start () {
		this.mk_inputParser = new InputParser();
	}

	void Update() {
		mk_inputParser.UpdateInputBuffers();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float deltaTime = Time.fixedDeltaTime;

		DriveCharacter(deltaTime);

		// Done with single-frame inputs
		mk_inputParser.ClearInputBuffers();
	}

	void DriveCharacter(float deltaTime){
		MovementState ms = kCharacter.movementState;
		switch(ms.currentTerrain) {
			case MovementState.TerrainNav.Ground:
			DriveCharacterOnGround(deltaTime);
			break;

			case MovementState.TerrainNav.Air:
			DriveCharacterInAir(deltaTime);
			break;

			case MovementState.TerrainNav.Wall:

			break;
		}
		kCharacter.UpdateState(Time.fixedDeltaTime);
	}

	void DriveCharacterOnGround(float deltaTime) {
		Vector2 userVelocity = mk_inputParser.GetDirection();
		MovementState ms = kCharacter.movementState;

		// Run or Slow down?
		if(userVelocity.x != 0f) {
			// Run
			Vector2 latVelocity = maxGroundSpeed * Vector2.right * userVelocity.x;
			ms.AccelerateLateral(latVelocity, groundAcceleration, deltaTime);
		}
		else {
			// Slow down
			bool beganThisUpdate = mk_inputParser.left.released || mk_inputParser.right.released;
			if(beganThisUpdate && ms.lateralSpeed > maxGroundDecelStartSpeed){
				ms.OverrideLateralSpeed(maxGroundDecelStartSpeed);
			}
			else{
				ms.AccelerateLateral(Vector2.zero, groundDeceleration, deltaTime);
			}
		}

		// Jump?
		if(mk_inputParser.space.pressed && 
			ms.currentTerrain == MovementState.TerrainNav.Ground) 
		{
			ms.LaunchForHeight(4f);
		}
	}

	void DriveCharacterInAir(float deltaTime) {
		MovementState ms = kCharacter.movementState;

		// Short-hop
		if(mk_inputParser.space.released && 
			ms.verticalSpeed > 0f)
		{
			ms.OverrideVerticalSpeed(ms.verticalSpeed * 0.5f);
		}
		else if(mk_inputParser.down.pressed && 
			0.3f*fastFallSpeed > ms.verticalSpeed && ms.verticalSpeed > -fastFallSpeed) 
		{
			// Fast-fall
			ms.OverrideVerticalSpeed(-fastFallSpeed);
		}

		// Air-drift
		Vector2 userVelocity = mk_inputParser.GetDirection();

		if(userVelocity.x == 0f){
			return; // Do nothing.
		}

		// At this point, we know the player wants to drift in the air laterally.
		Vector2 oldGroundedLatVelocity = ms.preJumpLateralVelocity;
		float oldGroundedLatSpeed = Mathf.Abs(oldGroundedLatVelocity.x);
		float maxVelocityDriftDelta = 0.4f * maxGroundSpeed;

		if(	oldGroundedLatSpeed > maxVelocityDriftDelta && 
			Mathf.Sign(userVelocity.x) != Mathf.Sign(oldGroundedLatVelocity.x) ) 
		{	
			// The player can only drift in the opposite direction the set out from.
			// otherwise they could accelerate fwd in the air.
			Vector2 driftDelta = Vector2.right * userVelocity.x * maxVelocityDriftDelta;
			Vector2 targetDriftVelocity = oldGroundedLatVelocity + driftDelta;
			ms.AccelerateLateral(targetDriftVelocity, 80f, deltaTime);
		}
		else if (oldGroundedLatSpeed <= maxVelocityDriftDelta ) {
			// Drift from a standstill can go in either direction!
			Vector2 targetDriftVelocity = Vector2.right * userVelocity.x * maxVelocityDriftDelta;
			ms.AccelerateLateral(targetDriftVelocity, 80f, deltaTime);
		}
	}
}
