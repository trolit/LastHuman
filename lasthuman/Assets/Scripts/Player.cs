﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D myrigidBody;
    private Animator myAnimator;

    [SerializeField]
    private float movementSpeed;

    private bool facingRight;
    private bool attack;

    [SerializeField]
    private Transform[] groundpoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;
    private bool Jump;
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
        if(attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAnimator.SetTrigger("attack");
            myrigidBody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            attack = true;
        }
        if(Input.GetKeyDown(KeyCode.Space))
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
