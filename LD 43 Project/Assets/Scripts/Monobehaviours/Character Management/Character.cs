using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public CharacterController kController;
	public Animation kAnimation;
	public Transform kBodyTransform;

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

		if(movementState.currentTerrain == MovementState.TerrainNav.Ground) {
			Vector3 tangentVelocity;
			bool platformThere = TerrainAnalysis(movementState, deltaTime, out tangentVelocity);
			if(platformThere){
				// Everything is fine (TM)
				kController.Move(moveDelta);
			}
			else {
				// EVICT!
				// By putting them into an aerial state.
				float vertComp = Mathf.Max(4f, tangentVelocity.y);
				movementState.Launch(vertComp);
				kController.Move(deltaTime * movementState.CalcTotalVelocity());
			}
		}
		else {
			kController.Move(moveDelta);
		}
	}

	// Return false if there's no plat ahead of us, true O.W.
	bool TerrainAnalysis(MovementState currMvmtState, float deltaTime, 
		out Vector3 tangentVelocity)
	{
		Vector3 rcOffset = Vector3.up * 0.25f;
		int lm = -1;
		float surveryDist = 0.75f;
		// Should only be here if we're grounded
		RaycastHit currentHit;
		bool hit = Physics.Raycast(
			transform.position + rcOffset,
			Vector3.down, 
			out currentHit,
			surveryDist, lm
		);
		if(hit == false){
			tangentVelocity = Vector3.zero;
			return false; // You shouldn't really hit this... hopefully.
		}

		tangentVelocity = CalcTangentVelocity(currentHit.normal, currMvmtState.lateralVelocity.x * Vector3.right);
		Vector3 tangentDelta = tangentVelocity * deltaTime;

		// Now we know where we'd be sans gravity.
		// Raycast AGAIN from there
		Vector3 noDropPosition = transform.position + rcOffset + tangentDelta;
		bool hit2 = Physics.Raycast(
			noDropPosition, Vector3.down,
			out currentHit,
			surveryDist, lm
		);
		if(hit2 == false){
			// THERE IS NO PLATFORM THERE!
			return false;
		}
		else {
			// There is a platform there. Assume it's not a bad angle.
			return true;
		}
	}

	Vector3 CalcTangentVelocity(Vector3 normal, Vector3 lateralVelocity) {
		Vector3 rightTangent = Vector3.Cross(normal, Vector3.forward);
		float csc = 1f / Vector3.ProjectOnPlane(rightTangent, Vector3.up).magnitude;
		float latMag = lateralVelocity.magnitude;
		return rightTangent * latMag * csc;
	}

	void OnControllerColliderHit(ControllerColliderHit hitInfo) {
		if(hitInfo.normal.y > 0.707f) {
			movementState.LandVertical();
		}
	}

	public void TurnTowards(Vector3 forward) {
		kBodyTransform.transform.rotation = Quaternion.Lerp(
			kBodyTransform.transform.rotation,
			Quaternion.LookRotation(forward),
			8f * Time.fixedDeltaTime
		);
	}

    /// <summary>
    /// Returns what direction, left or right the player is facing.
    /// </summary>
    /// It's a float because you can kind of be facing the middle.
    public float FacingDir {
        get {
            return kBodyTransform.transform.forward.x;
        }
    }
}
