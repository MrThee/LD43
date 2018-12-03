using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : AIController {
	public enum State {
		Idle,
		Attack
	}

	public State state {get; private set;}
	private System.Action<float> m_stateAction;

	private Vector3 m_faceDir;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.ChangeState(Idle, State.Idle);
		this.kCharacter.kAnimation.Play("Idle");
	}

	void FixedUpdate(){
		float deltaTime = Time.deltaTime;
		m_stateAction.Invoke(deltaTime);
		if(m_faceDir != Vector3.zero){
			kCharacter.TurnTowards(m_faceDir, deltaTime);
		}
		kCharacter.UpdateState(deltaTime);
	}

	void ChangeState(System.Action<float> callback, State state) {
		this.state = state;
		m_stateAction = callback;
	}

	void Idle(float deltaTime) {
		Vector3 toPlayer = GetToPlayer();
		if(toPlayer.magnitude < 7f){
			m_faceDir = Vector3.ProjectOnPlane(toPlayer, Vector3.up);
			if(Mathf.Abs(toPlayer.y) < 1f){
				kCharacter.kAnimation.CrossFade("Spit");
				ChangeState(Attack, State.Attack);
			}
		}
	}

	void Attack(float deltaTime) {
		if(kCharacter.kAnimation.IsPlaying("Spit") == false){
			kCharacter.kAnimation.CrossFade("Idle", 0.1f);
			ChangeState(Idle, State.Idle);
		}
	}
}
