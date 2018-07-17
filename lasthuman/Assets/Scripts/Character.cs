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

    [SerializeField]
    private EdgeCollider2D SwordCollider;

    public abstract bool IsDead { get; set; }
    // abstract property which means:
    // every single script needs their own implementation
    // of IsDead

    // contains every damage tag
    [SerializeField]
    private List<string> damageSources = new List<string>();

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

    public void MeleeAttack()
    {
        SwordCollider.enabled = !SwordCollider.enabled;
        // if disabled - enable
        // if enabled - disable
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    { 
        // if tag knife - take damage
        // but not implemented ability to
        // throw knife, in case leaving 
        // comment

        // check if list contains the tag
        // that we can take damage from
        if(damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
