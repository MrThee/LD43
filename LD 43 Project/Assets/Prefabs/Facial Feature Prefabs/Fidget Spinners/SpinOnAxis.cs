using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinOnAxis : MonoBehaviour {

	public Vector3 axis = Vector3.forward;
	public float omega = 720f;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(axis, omega*Time.deltaTime);
	}
}
