using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public LifeTimer kTrail;

	[System.Serializable]
	public class Params {
		public float lifeTime;
		public float speed;
		public int damage;
	}

	private OnForSeconds mk_lifeTimer;

	public Vector3 velocity {get; private set;}

	public void Kickoff(Params projParams, Vector3 origin, Vector3 direction){
		this.mk_lifeTimer = new OnForSeconds(projParams.lifeTime);
		this.mk_lifeTimer.deactivationEventHandler.Did.AddCallback(this.Destroy);
		this.transform.position = origin;
		this.velocity = direction * projParams.speed;

		mk_lifeTimer.ActivateForDefaultDuration();
	}

	void FixedUpdate(){
		float deltaTime = Time.fixedDeltaTime;
		mk_lifeTimer.UpdateState(deltaTime);

		Vector3 movementDelta = velocity * deltaTime;
		float deltaLength = movementDelta.magnitude;
		int lm = 0x801; // Hitboxes and default layer
		bool hit = Physics.Raycast(transform.position, velocity, deltaLength, lm);

		if(hit) {
			Destroy();
			return;
		}
		else {
			// Move it
			transform.position += velocity * deltaTime;
		}
	}

	void Destroy() {
		if(kTrail){
			kTrail.transform.SetParent(null);
			kTrail.enabled = true;
		}
		Destroy(this.gameObject);
	}
}
