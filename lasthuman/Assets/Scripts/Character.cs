using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    [SerializeField]
    protected float movementSpeed;

    protected bool facingRight;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    public Animator MyAnimator { get; private set; }

    [SerializeField]
    protected int health;

    public abstract bool IsDead { get; set; }
    // abstract property which means:
    // every single script needs their own implementation
    // of IsDead

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

    public abstract IEnumerator TakeDamage();
    // Enemy and Player needs to implement this function

    public virtual void OnTriggerEnter2D(Collider2D other)
    { 
}
}
