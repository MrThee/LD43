using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public enum State {
		Idle,
		InAir,
		Running
	}

	public Character kCharacter;
	public float maxGroundSpeed = 10f;
	public float maxGroundDecelStartSpeed = 5f;
	public float maxJumpHeight = 2f;
	public float fastFallSpeed = 16f;
	public float groundAcceleration = 80f;
	public float groundDeceleration = 120f;

	private InputParser mk_inputParser;

	public State state { get; private set;}
	private System.Action<float> m_stateAction;

	// Use this for initialization.. TODO: delet this.
	void Start () {
		this.mk_inputParser = new InputParser();
		this.state = State.Idle;
		this.m_stateAction = Idle;
		kCharacter.kAnimation.Play("Idle");
	}

	void Update() {
		mk_inputParser.UpdateInputBuffers();
	}
	
	void ChangePlayerState(System.Action<float> callback, State label){
		state = label;
		m_stateAction = callback;
	}

	// Update is called once per frame
	void FixedUpdate () {
		float deltaTime = Time.fixedDeltaTime;

		m_stateAction.Invoke(deltaTime);
		kCharacter.UpdateState(Time.fixedDeltaTime);

		// Done with single-frame inputs
		mk_inputParser.ClearInputBuffers();
	}

	void Idle(float deltaTime) {
		Vector2 userDirection = mk_inputParser.GetDirection();

		if(userDirection.x != 0f){
			kCharacter.kAnimation.CrossFade("Run", 0.1f);
			ChangePlayerState(Running, State.Running);
			return;
		}

		if(mk_inputParser.space.pressed) {
			kCharacter.movementState.LaunchForHeight(maxJumpHeight);
			kCharacter.kAnimation.Play("Jump");
			kCharacter.kAnimation.PlayQueued("InAir");
			ChangePlayerState(InAir, State.InAir);
			return;
		}

		kCharacter.movementState.AccelerateLateral(Vector2.zero, groundDeceleration, deltaTime);
		float latX = kCharacter.movementState.lateralVelocity.x;
		if(latX != 0f){
			kCharacter.TurnTowards(Mathf.Sign(latX) * Vector3.right);
		}
	}

	void Running(float deltaTime){
		Vector2 userDirection = mk_inputParser.GetDirection();
		MovementState ms = kCharacter.movementState;

		if(userDirection.x == 0f){
			kCharacter.kAnimation.CrossFade("Idle");
			if(ms.lateralSpeed > maxGroundDecelStartSpeed) {
				ms.OverrideLateralSpeed(maxGroundDecelStartSpeed);
			}
			ChangePlayerState(Idle, State.Idle);
			return;
		}

		if(mk_inputParser.space.pressed) {
			ms.LaunchForHeight(maxJumpHeight);
			kCharacter.kAnimation.Play("Jump");
			kCharacter.kAnimation.PlayQueued("InAir");
			ChangePlayerState(InAir, State.InAir);
			return;
		}

		// Keep movin'
		Vector2 latVelocity = maxGroundSpeed * Vector2.right * userDirection.x;
		ms.AccelerateLateral(latVelocity, groundAcceleration, deltaTime);
		kCharacter.TurnTowards(Vector3.right * latVelocity.x);
	}

	void InAir(float deltaTime) {
		// kCharacter handles gravity accleration.
		// This handles short-hops, fast-falls, and aerial drifting.
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
		Vector2 userDir = mk_inputParser.GetDirection();

		if(userDir.x == 0f){
			if(ms.lateralVelocity.x != 0f){
				kCharacter.TurnTowards(Vector3.right * ms.lateralVelocity.x);
			}
			return; // Do nothing.
		}

		kCharacter.TurnTowards(Vector3.right * userDir.x);

		// At this point, we know the player wants to drift in the air laterally.
		Vector2 oldGroundedLatVelocity = ms.preJumpLateralVelocity;
		float oldGroundedLatSpeed = Mathf.Abs(oldGroundedLatVelocity.x);
		float maxVelocityDriftDelta = 0.4f * maxGroundSpeed;

		if(	oldGroundedLatSpeed > maxVelocityDriftDelta && 
			Mathf.Sign(userDir.x) != Mathf.Sign(oldGroundedLatVelocity.x) ) 
		{	
			// The player can only drift in the opposite direction the set out from.
			// otherwise they could accelerate fwd in the air.
			Vector2 driftDelta = Vector2.right * userDir.x * maxVelocityDriftDelta;
			Vector2 targetDriftVelocity = oldGroundedLatVelocity + driftDelta;
			ms.AccelerateLateral(targetDriftVelocity, 80f, deltaTime);
		}
		else if (oldGroundedLatSpeed <= maxVelocityDriftDelta ) {
			// Drift from a standstill can go in either direction!
			Vector2 targetDriftVelocity = Vector2.right * userDir.x * maxVelocityDriftDelta;
			ms.AccelerateLateral(targetDriftVelocity, 80f, deltaTime);
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hitInfo) {
		switch(state){
			case State.InAir:
			if(hitInfo.normal.y > 0.707f) {
				if(kCharacter.movementState.lateralSpeed > 4f){
					// TODO: less classes that can control kAnimator.
					kCharacter.kAnimation.Play("Land2Run");
					kCharacter.kAnimation.CrossFade("Run");
					ChangePlayerState(Running, State.Running);
				}
				else{
					kCharacter.kAnimation.Play("Land2Idle");
					kCharacter.kAnimation.CrossFade("Idle");
					ChangePlayerState(Idle, State.Idle);
				}
			}
			break;
		}
	}
}
