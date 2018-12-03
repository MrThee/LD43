using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterDriver : MonoBehaviour {
    public Character kCharacter;
    public int maxHP = 1;
    public int hp {get; protected set;}
    protected virtual void Start(){
        this.hp = maxHP;
    }
    public virtual void TakeDamage(int damage, Vector3 direction) {
        hp -= damage;
        if(hp <= 0){
            Destroy(this.gameObject);
        }
    }
}