using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
public class MovementState {

	// Infrequently-changing fields
	public float gravityMagnitude {get; private set;}

	// Frame-by-frame dynamic fields
	public Vector2 lateralVelocity {get; private set;}
	public float lateralSpeed {get { return lateralVelocity.magnitude; } }

	public float verticalSpeed {get; private set;}

	private float m_clingSpeed;

	// Handy cached fields
	// Populated after currentTerrainNav
	public Vector2 preJumpLateralVelocity {get; set;}

	public enum TerrainNav {
		Ground,
		Wall,
		Air
	}
	public TerrainNav currentTerrain {get; private set;}

	public static float DefaultGravity { get { return 40f; } }

	public MovementState () {
		this.lateralVelocity = Vector2.zero;
		this.gravityMagnitude = MovementState.DefaultGravity;
		this.currentTerrain = TerrainNav.Ground;
		this.m_clingSpeed = 40f;
	}

	public Vector3 CalcTotalVelocity(bool excludeClingSpeed=false) {
		Vector2 vertComp = verticalSpeed * Vector2.up;
		Vector2 total2D = lateralVelocity + vertComp;
		if(currentTerrain == TerrainNav.Ground && !excludeClingSpeed) {
			// Add a cling-speed component.
			total2D += Vector2.down * m_clingSpeed;
		}
		return new Vector3(total2D.x, total2D.y); // z will always be 0 w/ this constructor.
	}

	public void MaybeApplyGravity(float deltaTime) {
		switch(currentTerrain) {
			default:
			case TerrainNav.Ground:
			break;

			case TerrainNav.Wall:
			case TerrainNav.Air:
			verticalSpeed -= deltaTime * gravityMagnitude;
			break;
		}
	}

	// Call when the using controller hits the ground.
	public void LandVertical() {
		currentTerrain = TerrainNav.Ground;
		verticalSpeed = 0f;
	}

	public void LandWall() {
		currentTerrain = TerrainNav.Wall;
		OverrideLateralSpeed(Mathf.Min(lateralSpeed*0.5f, 1f));
	}

	public void LaunchForHeight(float heightDelta) {
		// 0.5 * mass * velocity ^2 = mass * gravity * height
		// velocity = sqrt(2*g*h)
		currentTerrain = TerrainNav.Air;
		verticalSpeed = Mathf.Sqrt(heightDelta * gravityMagnitude * 2f);
		preJumpLateralVelocity = lateralVelocity;
	}

	public void Launch(float vertSpeed){
		currentTerrain = TerrainNav.Air;
		OverrideVerticalSpeed(vertSpeed);
	}

	public void OverrideVerticalSpeed(float newVertSpeed){
		verticalSpeed = newVertSpeed;
		preJumpLateralVelocity = lateralVelocity;
	}

	public void OverrideLateralSpeed(float newLatSpeed){
		Vector2 currentLatDir = lateralVelocity.normalized;
		lateralVelocity = currentLatDir * newLatSpeed;
	}

    public void OverrideLateralVelocity(float newHorzVelocityComp) {
        lateralVelocity = Vector2.right * newHorzVelocityComp;
    }

    public void AccelerateLateral(Vector2 finalLateralVelocity, float rate, float deltaTime)
    {
        lateralVelocity = Vector2.MoveTowards(lateralVelocity, finalLateralVelocity, rate * deltaTime);
    }

    public void DampenLateral(float rate, float deltaTime)
    {
        lateralVelocity = (1 - rate) * lateralVelocity;
    }
}
