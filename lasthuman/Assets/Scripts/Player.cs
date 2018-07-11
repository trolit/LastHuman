using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

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

    private Animator myAnimator;

    [SerializeField]
    private float movementSpeed;

    private bool facingRight;

    public Rigidbody2D myrigidBody;

    public bool attack;
    private bool jumpattack;

    [SerializeField]
    private Transform[] groundpoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    public bool isGrounded;
    public bool Jump;
    [SerializeField]
    private bool aircontrol;

    [SerializeField]
    private float JumpForce;


    // Use this for initialization
    void Start () {
        facingRight = true;
        myrigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        HandleInput();
    }

	void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = isgrounded();

        HandleMovement(horizontal);
        Flip(horizontal);
        HandleAttacks();
        HandleLayers();
        ResetValues();
	}

    private void HandleMovement(float horizontal)
    {
        if (myrigidBody.velocity.y < 0 )
        {
            myAnimator.SetBool("land", true);
        }
        if (isGrounded && Jump == true)
        {
            isGrounded = false;
            myrigidBody.AddForce(new Vector2(0, JumpForce));
            myAnimator.SetTrigger("jump");
        }
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGrounded || aircontrol))
        {
            myrigidBody.velocity = new Vector2(horizontal * movementSpeed, myrigidBody.velocity.y);
        }

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleAttacks()
    {
        if (attack && isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAnimator.SetTrigger("attack");
            myrigidBody.velocity = Vector2.zero;
        }
        if (jumpattack && !isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("jumpattack"))
        {
            // myAnimator.SetBool("jumpattack", true);
            // instead of bool, trigger fixed double animation problem
            myAnimator.SetBool("jumpattack", true);

        }
        if (!jumpattack && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("jumpattack"))
        {
            myAnimator.SetBool("jumpattack", false);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            attack = true;
            jumpattack = true;
        }
        if(GameInputManager.GetKeyDown("Jump"))
        {
            Jump = true;
        }
    }

    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    // reset values
    private void ResetValues()
    {
        attack = false;
        Jump = false;
        jumpattack = false;
    }

    private bool isgrounded()
    {
        if (myrigidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundpoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
}
