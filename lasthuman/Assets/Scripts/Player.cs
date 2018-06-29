using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D myrigidBody;
    private Animator myAnimator;

    [SerializeField]
    private float movementSpeed;

    private bool facingRight;

	// Use this for initialization
	void Start () {
        facingRight = true;
        myrigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");

        HandleMovement(horizontal);
        Flip(horizontal);
	}

    private void HandleMovement(float horizontal)
    {
        myrigidBody.velocity = new Vector2(horizontal * movementSpeed, myrigidBody.velocity.y);

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
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
}
