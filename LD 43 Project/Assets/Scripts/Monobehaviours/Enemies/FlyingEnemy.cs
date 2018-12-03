using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : AIController {
    
    public enum State
    {
        Idle,
        Attack
    }
    private Rigidbody body;

    public State state { get; private set; }
    private System.Action<float> m_stateAction;

    public float speed = 1f;
    private Vector3 velocity = new Vector3();

	// Use this for initialization
	protected override void Start(){
        base.Start();
        body = GetComponent<Rigidbody>();

        this.ChangeState(Idle, State.Idle);
    }

    void FixedUpdate()
    {
        float deltaTime = Time.deltaTime;
        m_stateAction.Invoke(deltaTime);
        kCharacter.UpdateState(deltaTime);
    }

    void ChangeState(System.Action<float> callback, State state)
    {
        this.state = state;
        m_stateAction = callback;
    }

    void Idle(float deltaTime)
    {
        Vector3 toPlayer = GetToPlayer();
        if (toPlayer.magnitude < 7f)
        {
            ChangeState(Attack, State.Attack);
        }
    }

	// Update is called once per frame
	void Attack (float deltaTime) {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 desiredVelocity = speed * direction;
        velocity = Vector3.Slerp(velocity, desiredVelocity, 0.1f);
        body.velocity = velocity;
	}

    private void OnTriggerEnter(Collider other)
    {
        PlayerController hitPlayer = other.gameObject.GetComponent<PlayerController>();
        if (hitPlayer) {
            hitPlayer.TakeDamage(1, hitPlayer.transform.position - transform.position);
        }
    }
}
