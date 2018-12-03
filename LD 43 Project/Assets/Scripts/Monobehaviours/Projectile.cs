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
	private int mk_damage;

	public Vector3 velocity {get; private set;}

	public void Kickoff(Params projParams, Vector3 origin, Vector3 direction){
		this.mk_lifeTimer = new OnForSeconds(projParams.lifeTime);
		this.mk_lifeTimer.deactivationEventHandler.Did.AddCallback(this.Destroy);
		this.mk_damage = projParams.damage;
		this.transform.position = origin;
		this.velocity = direction * projParams.speed;

		mk_lifeTimer.ActivateForDefaultDuration();
	}

	void FixedUpdate(){
		float deltaTime = Time.fixedDeltaTime;
		mk_lifeTimer.UpdateState(deltaTime);

		Vector3 movementDelta = velocity * deltaTime;
		float deltaLength = movementDelta.magnitude;
		int lm = 0x201; // Hitboxes and default layer
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, velocity, out hitInfo, deltaLength, lm, QueryTriggerInteraction.Collide);

		if(hit) {
            TryDoDamage(hitInfo);

			Destroy();
			return;
		}
		else {
			// Move it
			transform.position += velocity * deltaTime;
		}
	}

    void TryDoDamage(RaycastHit hitInfo) {
        Hitbox hitbox = hitInfo.collider.GetComponent<Hitbox>();
        if (hitbox == null) {
            return;
        }
        hitbox.TakeDamage(mk_damage, velocity);
    }

	void Destroy() {
		if(kTrail){
			kTrail.transform.SetParent(null);
			kTrail.enabled = true;
		}
		Destroy(this.gameObject);
	}
}
