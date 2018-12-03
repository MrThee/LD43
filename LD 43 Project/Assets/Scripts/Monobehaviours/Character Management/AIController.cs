using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : CharacterDriver {

    protected PlayerController player;

    protected override void Start(){
        player = FindObjectOfType<PlayerController>();
    }

	protected Vector3 GetToPlayer(){
		return player.transform.position - transform.position;
	}
}
