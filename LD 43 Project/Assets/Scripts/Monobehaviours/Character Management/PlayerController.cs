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
	public InventoryHarness kInventoryHarness;
	[Header("Movement")]
	public float maxGroundSpeed = 10f;
	public float maxGroundDecelStartSpeed = 5f;
	public float maxJumpHeight = 2f;
	public float wallJumpHorzSpeed = 8f;
	public float fastFallSpeed = 16f;
	public float groundAcceleration = 80f;
	public float groundDeceleration = 100f;
    public float groundDamping = 0.05f;

    public float dashSpeed = 15f;
    public bool canDash = true;

	[Header("Combat")]
	public Transform firePoint;

    private GameStateHandler gameStateHandler;
	private InputParser mk_inputParser;
	private OnForSeconds mk_fireCooldown;
	private Vector3 m_intendedFacingDirection; // Never assign Vector3.zero to this.

	public State state { get; private set;}
	private System.Action<float> m_stateAction;

    // Health things
    private int health;

	// Use this for initialization.. TODO: delet this.
	void Start () {
		this.mk_inputParser = new InputParser();
		this.mk_fireCooldown = new OnForSeconds(0.25f);
		this.state = State.Idle;
		this.m_stateAction = Idle;
		// only time we use
		// planar forward for this field
		this.m_intendedFacingDirection = kCharacter.planarForward; 
		kCharacter.kAnimation.Play("Idle");

        gameStateHandler = FindObjectOfType<GameStateHandler>();
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

		MovementState.TerrainNav oldTerrain = kCharacter.movementState.currentTerrain;
		kCharacter.UpdateState(Time.fixedDeltaTime);
		MovementState.TerrainNav newTerrain = kCharacter.movementState.currentTerrain;
		if(	oldTerrain == MovementState.TerrainNav.Ground && 
			newTerrain == MovementState.TerrainNav.Air)
		{
			kCharacter.kAnimation.Play("Jump");
			kCharacter.kAnimation.PlayQueued("InAir");
			ChangePlayerState(InAir, State.InAir);
		}

		mk_fireCooldown.UpdateState(deltaTime);
		if(m_intendedFacingDirection != Vector3.zero){
			kCharacter.TurnTowards(m_intendedFacingDirection, deltaTime);
		}

		// Done with single-frame inputs
		mk_inputParser.ClearInputBuffers();
	}

    void Idle(float deltaTime) {
        Vector2 userDirection = Vector2.zero;
        if (gameStateHandler.state == GameStateHandler.GameState.GamePlay)
        {
            userDirection = mk_inputParser.GetDirection();
        }

		if(userDirection.x != 0f){
			kCharacter.kAnimation.CrossFade("Run", 0.1f);
			ChangePlayerState(Running, State.Running);
			return;
		}

        if (gameStateHandler.state == GameStateHandler.GameState.GamePlay) {
            if (mk_inputParser.shift.pressed)
            {
                StartDash();
            }
            if (mk_inputParser.space.pressed)
            {
                kCharacter.movementState.LaunchForHeight(maxJumpHeight);
                kCharacter.kAnimation.Play("Jump");
                kCharacter.kAnimation.PlayQueued("InAir");
                ChangePlayerState(InAir, State.InAir);
                return;
            }

			if(kInventoryHarness.currentGun) {
				if(mk_inputParser.mouse0.pressed && mk_fireCooldown.active == false){
					// FIRE!
					kCharacter.kAnimation.Stop();
					kCharacter.kAnimation.Play("FireFromIdle");
					kCharacter.kAnimation.CrossFadeQueued("Idle", 0.1f);
					kInventoryHarness.currentGun.Launch(firePoint.position, kCharacter.planarForward);
					mk_fireCooldown.ActivateForDefaultDuration();
				}
			}
        }

        DampenSpeed(deltaTime);

		float latX = kCharacter.movementState.lateralVelocity.x;
		if(latX != 0f) {
			m_intendedFacingDirection = latX * Vector3.right;
		}
	}

    void DampenSpeed(float deltaTime) {
        kCharacter.movementState.DampenLateral(groundDamping, deltaTime);
        kCharacter.movementState.AccelerateLateral(Vector2.zero, groundDeceleration, deltaTime);
    }

	void Running(float deltaTime){
        Vector2 userDirection = Vector2.zero;
        if (gameStateHandler.state == GameStateHandler.GameState.GamePlay) {
            userDirection = mk_inputParser.GetDirection();

			if(kInventoryHarness.currentGun){
				if(mk_inputParser.mouse0.pressed && mk_fireCooldown.active == false){
					// FIRE!
					kCharacter.kAnimation.Play("FireFromRun");
					// kCharacter.kAnimation.CrossFadeQueued("Idle", 0.1f);
					kInventoryHarness.currentGun.Launch(firePoint.position, kCharacter.planarForward);
					mk_fireCooldown.ActivateForDefaultDuration();
				}
			}
        }
		MovementState ms = kCharacter.movementState;

		if(userDirection.x == 0f){
			kCharacter.kAnimation.CrossFade("Idle");
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
        if (mk_inputParser.shift.pressed)
        {
            StartDash();
        }

		// Keep movin'
		Vector2 latVelocity = maxGroundSpeed * Vector2.right * userDirection.x;
		ms.AccelerateLateral(latVelocity, groundAcceleration, deltaTime);
		if(latVelocity.x != 0f){
			m_intendedFacingDirection = latVelocity.x * Vector3.right;
		}
	}

	void InAir(float deltaTime) {
		// kCharacter handles gravity accleration.
		// This handles short-hops, fast-falls, and aerial drifting.
		MovementState ms = kCharacter.movementState;

        if (gameStateHandler.state == GameStateHandler.GameState.GamePlay) {
            // Short-hop
            if (mk_inputParser.space.released &&
                ms.verticalSpeed > 0f)
            {
                ms.OverrideVerticalSpeed(ms.verticalSpeed * 0.5f);
            }
            else if (mk_inputParser.down.pressed &&
                0.3f * fastFallSpeed > ms.verticalSpeed && ms.verticalSpeed > -fastFallSpeed)
            {
                // Fast-fall
                ms.OverrideVerticalSpeed(-fastFallSpeed);
            }

			// Dash
            if (mk_inputParser.shift.pressed && canDash)
            {
                canDash = false;
                StartDash();
            }

			// Wall-jump
			if(	mk_inputParser.space.pressed && 
				ms.currentTerrain == MovementState.TerrainNav.Wall) 
			{
				kCharacter.kAnimation.Play("Jump");
				kCharacter.kAnimation.Play("InAir");
				float horzComp = wallJumpHorzSpeed * Mathf.Sign(kCharacter.lastWallNormal.x);
				ms.OverrideLateralVelocity(horzComp);
				ms.LaunchForHeight(maxJumpHeight*0.4f);
				// Don't really need to change state.
				m_intendedFacingDirection = horzComp * Vector3.right;
				kCharacter.Face(m_intendedFacingDirection);
			}

			// !GUN!
			if(kInventoryHarness.currentGun){
				if(mk_inputParser.mouse0.pressed && mk_fireCooldown.active == false){
					// FIRE!
					kCharacter.kAnimation.Stop();
					kCharacter.kAnimation.Play("FireFromAir");
					kCharacter.kAnimation.CrossFadeQueued("InAir", 0.1f);
					kInventoryHarness.currentGun.Launch(firePoint.position, kCharacter.planarForward);
					mk_fireCooldown.ActivateForDefaultDuration();
				}
			}
        }

        // Air-drift
        Vector2 userDir = Vector2.zero;
        if (gameStateHandler.state == GameStateHandler.GameState.GamePlay) {
            userDir = mk_inputParser.GetDirection();
        }

		if(userDir.x == 0f){
			if(ms.lateralVelocity.x != 0f){
				m_intendedFacingDirection = Vector3.right * ms.lateralVelocity.x;
			}
			return; // Do nothing.
		}

		m_intendedFacingDirection = userDir.x * Vector3.right;

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
                if (hitInfo.normal.y > 0.707f)
                {
                    // Reset things that happen when you land here
                    canDash = true;

                    if (kCharacter.movementState.lateralSpeed > 4f)
                    {
                        // TODO: less classes that can control kAnimator.
                        kCharacter.kAnimation.Play("Land2Run");
                        kCharacter.kAnimation.CrossFade("Run");
                        ChangePlayerState(Running, State.Running);
                    }
                    else
                    {
                        kCharacter.kAnimation.Play("Land2Idle");
                        kCharacter.kAnimation.CrossFade("Idle");
                        ChangePlayerState(Idle, State.Idle);
                    }
                }
			break;
		}
	}

    void StartDash()
    {
        kCharacter.movementState.OverrideVerticalSpeed(5f);

        float dashVelocity = GetDashVelocity();
        kCharacter.movementState.OverrideLateralVelocity(dashVelocity);
    }

    float GetDashVelocity()
    {
        float xUserInput = mk_inputParser.GetDirection().x;
        if (xUserInput < 0)
        {
            return -dashSpeed;
        }
        else if (xUserInput > 0)
        {
            return dashSpeed;
        }

        float facingDir = kCharacter.FacingDir;
        if (facingDir < 0)
        {
            return -dashSpeed;
        }
        else if (facingDir > 0)
        {
            return dashSpeed;
        }
        // just pick right if there's an absolute tie
        return dashSpeed;
    }

    public void TakeDamage(int amount, Vector3 direction) {
        health -= amount;
        // Just quickly push the player back a little. Hopefully doesn't break things.
    }

}
