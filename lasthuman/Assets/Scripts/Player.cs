using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    // variables start with small letter
    private static Player instance;

    // player singleton
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }





    [SerializeField]
    private Transform[] groundpoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool aircontrol;

    [SerializeField]
    private float JumpForce;

    public Rigidbody2D MyRigidbody { get; set; }
    
    public bool Jump { get; set; }
    public bool OnGround { get; set; }
    // properties start with capital letter!!!

    private Vector2 startPos;

    // Use this for initialization
    public override void Start ()
    {
        // call Start function from Character
        base.Start();

        startPos = transform.position;
        MyRigidbody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update () {
        HandleInput();
    }

	void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");
        OnGround = IsGrounded();

        HandleMovement(horizontal);
        Flip(horizontal);
        HandleLayers();
	}

    private void HandleMovement(float horizontal)
    {
        if(MyRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }

        if(!Attack && (OnGround || aircontrol))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }

        if(Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, JumpForce));
        }

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            myAnimator.SetTrigger("attack");
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetTrigger("jump");
        }
    }

    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }

    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundpoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
}
