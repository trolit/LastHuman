using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    [SerializeField]
    protected float movementSpeed;

    protected bool facingRight;

    public bool Attack { get; set; }

    public Animator MyAnimator { get; private set; }

    // Use this for initialization
    public virtual void Start ()
    {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        // does the same as the functionality in Flip function
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);

        /* Flip function: 
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale; 
        */
    }
}
