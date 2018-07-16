using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // variable to watch state
    private IEnemyState currentState;

    // property
    public GameObject Target { get; set; }

    [SerializeField]
    private float MeleeRange;

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                // if range is enough to do melee attack return true
                return Vector2.Distance(transform.position, Target.transform.position) <= MeleeRange;
            }

            // else false
            return false;
        }
    }

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        ChangeState(new IdleState());
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentState.Execute();
        LookAtTarget();
	}

    // keep an eye on player
    private void LookAtTarget()
    {
        if(Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;
            if(xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();

        }

        //change current state to a new state
        currentState = newState;

        // enter new state
        currentState.Enter(this);
    }

    public void Move()
    {
        // if attack is false, we cant move
        if (!Attack)
        {
            // to avoid sliding
            MyAnimator.SetFloat("speed", 1);

            transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
        }
    }

    public Vector2 GetDirection()
    {
        // short form of if statement
        return facingRight ? Vector2.right : Vector2.left;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        currentState.OnTriggerEnter(other);
    }


}
