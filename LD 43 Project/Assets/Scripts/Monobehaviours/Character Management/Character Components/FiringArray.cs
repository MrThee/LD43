using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringArray : MonoBehaviour {

	public Projectile pPrefab;
	public Projectile.Params pProps;
	public List<Transform> barrels = new List<Transform>();
	public float roundDelta;
	public GameObject nozzleFlare;

	private List<Vector3> mf_origins;
	private List<Vector3> mf_dirs;

	public void Start(){
		this.mf_origins = new List<Vector3>(barrels.Count);
		this.mf_dirs = new List<Vector3>(barrels.Count);
	}

	public void Launch() {
		mf_dirs.Clear();
		mf_origins.Clear();
		foreach(Transform t in barrels){
			mf_origins.Add(t.position);
			mf_dirs.Add(t.forward);
		}
	}

	public void Launch(Vector3 origin, Vector3 direction) {
		mf_dirs.Clear();
		mf_origins.Clear();
		for(int i = 0; i < barrels.Count; i++){
			mf_origins.Add(origin);
			mf_dirs.Add(direction);
		}
	}

	public void Launch(Vector3 direction) {
		mf_dirs.Clear();
		mf_origins.Clear();
		for(int i = 0; i < barrels.Count; i++){
			mf_origins.Add(barrels[i].position);
			mf_dirs.Add(direction);
		}
	}

	void LateUpdate(){
		// Most barrels animate, so we don't really launch until here.
		if(mf_origins.Count > 0){
			StartCoroutine(FireArrays(
				this.pPrefab,
				this.pProps,
				mf_origins, mf_dirs,
				this.roundDelta,
				this.nozzleFlare
			));
			mf_origins.Clear();
			mf_dirs.Clear();
		}
	}

	IEnumerator FireArrays(
		Projectile projectilePrefab,
		Projectile.Params projectileParameters,
		List<Vector3> projectileOrigins,
		List<Vector3> projectileDirections,
		float timeBetweenBullets,
		GameObject nozzleFlarePrefab
	){
		WaitForSeconds waiter = timeBetweenBullets > 0f ? new WaitForSeconds(timeBetweenBullets) : null;

		for(int i = 0; i < projectileOrigins.Count; i++){
			Vector3 currentOrigin = projectileOrigins[i];
			Vector3 spawnPoint = new Vector3(currentOrigin.x, currentOrigin.y);
			Vector3 direction = new Vector3(projectileDirections[i].x, projectileDirections[i].y).normalized;
			Quaternion spawnRot = Quaternion.LookRotation(direction);

			// Spawn stuff
			if(nozzleFlarePrefab){
				Instantiate<GameObject>(nozzleFlarePrefab, spawnPoint, spawnRot);
			}
			Projectile newProjectile = Instantiate<Projectile>(projectilePrefab, spawnPoint, Quaternion.LookRotation(direction));
			newProjectile.Kickoff(projectileParameters, spawnPoint, direction);

			if(waiter != null){
				yield return waiter;
			}
			// Fire the next round.
		}
		yield return null;
	}
}
