using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterDriver : MonoBehaviour {
    public Character kCharacter;
    public RendererWrapper kRendWrap;
    public GameObject deathEffect;
    public int maxHP = 1;
    public int hp {get; protected set;}
    protected virtual void Start(){
        this.hp = maxHP;
    }
    public virtual void TakeDamage(int damage, Vector3 direction) {
        hp -= damage;
        if(hp <= 0){
            if(deathEffect){
                Instantiate(deathEffect, transform.position + 0.5f*Vector3.up, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}