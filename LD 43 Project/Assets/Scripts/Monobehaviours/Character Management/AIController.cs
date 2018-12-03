using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : CharacterDriver {

    protected PlayerController player;

    protected override void Start(){
        base.Start();
        player = FindObjectOfType<PlayerController>();
    }

	protected Vector3 GetToPlayer(){
		return player.transform.position - transform.position;
	}

    public override void TakeDamage(int damage, Vector3 direction){
        kRendWrap.FlashRed();
        base.TakeDamage(damage, direction);
    }
}
