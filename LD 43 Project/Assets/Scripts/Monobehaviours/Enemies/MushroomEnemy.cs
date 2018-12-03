using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : MonoBehaviour {
	public Character kCharacter;

	private PlayerController mk_player;

	public enum State {
		Idle,
		Attack
	}

	public State state {get; private set;}
	private System.Action<float> m_stateAction;

	// Use this for initialization
	void Start () {
		this.mk_player = FindObjectOfType<PlayerController>();
		this.ChangeState(Idle, State.Idle);
		this.kCharacter.kAnimation.Play("Idle");
	}

	void FixedUpdate(){
		float deltaTime = Time.deltaTime;
		m_stateAction.Invoke(deltaTime);
		kCharacter.UpdateState(deltaTime);
	}

	void ChangeState(System.Action<float> callback, State state) {
		this.state = state;
		m_stateAction = callback;
	}

	void Idle(float deltaTime) {
		Vector3 toPlayer = GetToPlayer();
		if(toPlayer.magnitude < 7f && Mathf.Abs(toPlayer.y) < 1f){
			kCharacter.TurnTowards(Vector3.ProjectOnPlane(toPlayer, Vector3.up), deltaTime);
			kCharacter.kAnimation.CrossFade("Spit");
			ChangeState(Attack, State.Attack);
		}
	}

	void Attack(float deltaTime) {
		if(kCharacter.kAnimation.IsPlaying("Spit") == false){
			kCharacter.kAnimation.CrossFade("Idle", 0.1f);
			ChangeState(Idle, State.Idle);
		}
	}

	private Vector3 GetToPlayer(){
		return mk_player.transform.position - transform.position;
	}
}
