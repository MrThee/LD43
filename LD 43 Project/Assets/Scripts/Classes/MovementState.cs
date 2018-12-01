using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState {

	// Infrequently-changing fields
	public float gravityMagnitude {get; private set;}

	// Frame-by-frame dynamic fields
	public Vector2 lateralVelocity {get; private set;}
	public float lateralSpeed {get { return lateralVelocity.magnitude; } }

	public float verticalSpeed {get; private set;}


	public enum TerrainNav {
		Ground,
		Wall,
		Air
	}
	public TerrainNav currentTerrain {get; private set;}

	public static float DefaultGravity { get { return 80f; } }

	public MovementState () {
		this.lateralVelocity = Vector2.zero;
		this.gravityMagnitude = MovementState.DefaultGravity;
		this.currentTerrain = TerrainNav.Ground;
	}

	public Vector3 CalcTotalVelocity() {
		Vector2 vertComp = verticalSpeed * Vector2.up;
		Vector2 total2D = lateralVelocity + vertComp;
		return new Vector3(total2D.x, total2D.y); // z will always be 0 w/ this constructor.
	}

	public void MaybeApplyGravity(float deltaTime) {
		switch(currentTerrain) {
			default:
			case TerrainNav.Ground:
			break;

			case TerrainNav.Air:
			verticalSpeed -= deltaTime * gravityMagnitude;
			break;

			case TerrainNav.Wall:
			// TODO: edit w/ infrequently-changing "wall friction" field
			// Maybe instead of downscaling gravity's influence, the deceleration
			// should be proportional to the character's current speed,
			// more akin to a terminal-velocity equation. Could also do a same thing for air...
			// F_friction ~ k * speed
			verticalSpeed -= deltaTime * gravityMagnitude * 0.25f;
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
		lateralVelocity = Vector2.zero;
	}

	public void LaunchForHeight(float heightDelta) {
		// 0.5 * mass * velocity ^2 = mass * gravity * height
		// velocity = sqrt(2*g*h)
		verticalSpeed = Mathf.Sqrt(heightDelta * gravityMagnitude * 2f);
		currentTerrain = TerrainNav.Air;
	}

	public void AccelerateLateral(Vector2 finalLateralVelocity, float rate, float deltaTime) {
		lateralVelocity = Vector2.MoveTowards(lateralVelocity, finalLateralVelocity, rate*deltaTime);
	}
}
