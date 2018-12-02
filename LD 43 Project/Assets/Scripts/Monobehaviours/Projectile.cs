using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

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
		this.mk_lifeTimer.deactivationEventHandler.Did.AddCallback(this.OnLifeTimerExpired);
		this.transform.position = origin;
		this.velocity = direction * projParams.speed;

		mk_lifeTimer.ActivateForDefaultDuration();
	}

	void FixedUpdate(){
		float deltaTime = Time.fixedDeltaTime;
		mk_lifeTimer.UpdateState(deltaTime);
	}

	// ya boi ain't got time for poolin' this.
	void OnLifeTimerExpired(){
		Destroy(this.gameObject);
	}
}
