using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringArray : MonoBehaviour {

	public Projectile pPrefab;
	public Projectile.Params pProps;
	public List<Transform> barrels = new List<Transform>();
	public float roundDelta;
	public GameObject nozzleFlare;

	public void Launch() {
		StartCoroutine(FireArrays());
	}

	IEnumerator FireArrays(){
		WaitForSeconds waiter = this.roundDelta > 0f ? new WaitForSeconds(this.roundDelta) : null;

		for(int i = 0; i < barrels.Count; i++){
			Transform currentBarrel = barrels[i];
			Vector3 spawnPoint = new Vector3(currentBarrel.position.x, currentBarrel.position.y);
			Vector3 direction = new Vector3(currentBarrel.forward.x, currentBarrel.forward.y).normalized;
			Quaternion spawnRot = Quaternion.LookRotation(direction);

			// Spawn stuff
			if(nozzleFlare){
				Instantiate<GameObject>(nozzleFlare, spawnPoint, spawnRot);
			}
			Projectile newProjectile = Instantiate<Projectile>(pPrefab, spawnPoint, Quaternion.LookRotation(direction));
			newProjectile.Kickoff(this.pProps, spawnPoint, direction);

			if(waiter != null){
				yield return waiter;
			}
			// Fire the next round.
		}
		yield return null;
	}
}
