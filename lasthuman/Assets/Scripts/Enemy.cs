using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // variable to watch state
    private IEnemyState currentState;

    // property Target to set enemy Target
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

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }

        set
        {

        }
    }

    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;

    // Use this for initialization
    public override void Start ()
    {
        base.Start();

        // RemoveTarget function called whenever Players Dead event is triggered
        // we can access Player instance cause we have that singleton...
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);

        ChangeState(new IdleState());
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if not dead
        if(!IsDead)
        {
            if(!TakingDamage)
            {
                currentState.Execute();
            }
            // look at target if taking damage
            LookAtTarget();
        }
	}

    public void RemoveTarget()
    {
        // if enemy kills player
        // goes into Patrol State
        // and loses target
        Target = null;
        ChangeState(new PatrolState());
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
            // can move unless on the edge
            if((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > rightEdge.position.x))
            {
                // to avoid sliding
                MyAnimator.SetFloat("speed", 1);

                transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
            }
            // if our current state is PatrolState
            // Change direction so we can move and
            // continue patrolling
            else if(currentState is PatrolState)
            {
                ChangeDirection();
            }
        }
    }

    public Vector2 GetDirection()
    {
        // short form of if statement
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;
        // for now -10 damage , later maybe random?

        if(!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }

    public override void Death()
    {
        Destroy(gameObject);
    }
}
