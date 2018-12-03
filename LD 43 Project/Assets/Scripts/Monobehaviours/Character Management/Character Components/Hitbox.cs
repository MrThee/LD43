using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

	public CharacterDriver owner;

	public void TakeDamage(int damage, Vector3 direction){
		owner.TakeDamage(damage, direction);
	}
}
