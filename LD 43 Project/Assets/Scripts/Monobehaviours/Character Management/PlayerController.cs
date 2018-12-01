using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Character kCharacter;

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
		Vector2 userVelocity = 10f * mk_inputParser.GetDirection();
		Vector2 latVelocity = Vector2.right * userVelocity.x;

		kCharacter.movementState.AccelerateLateral(latVelocity, 80f, deltaTime);
		if(mk_inputParser.jumpPressed && 
			kCharacter.movementState.currentTerrain == MovementState.TerrainNav.Ground) 
		{
			kCharacter.movementState.LaunchForHeight(4f);
		}
		kCharacter.UpdateState(Time.fixedDeltaTime);

		// Done with single-frame inputs
		mk_inputParser.ClearInputBuffers();
	}
}
