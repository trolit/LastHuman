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

    private bool immortal = false;

    [SerializeField]
    private float immortalTime;

    private SpriteRenderer spriteRenderer;

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

    // properties start with capital letter!!!

    private Vector2 startPos;

    // Use this for initialization
    public override void Start ()
    {
        // call Start function from Character
        base.Start();

        startPos = transform.position;
        MyRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update () {
        if(!TakingDamage && !IsDead)
        {
            HandleInput();
        }
    }

	void FixedUpdate () {
        // if we are not taking damage and we are not dead we can do things...
        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            OnGround = IsGrounded();

            HandleMovement(horizontal);
            Flip(horizontal);
            HandleLayers();
        }
	}

    private void HandleMovement(float horizontal)
    {
        if(MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }

        if(!Attack && (OnGround || aircontrol))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }

        if(Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, JumpForce));
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            MyAnimator.SetTrigger("attack");
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MyAnimator.SetTrigger("jump");
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
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    private IEnumerator IndicateImmortal()
    {
        // as long as we are immortal...
        while(immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        // if we are not immortal
        if(!immortal)
        {
            health -= 10;

            // Debug.Log("i got damage :(");

            // if we are not dead
            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;

                StartCoroutine(IndicateImmortal());

                // wait immortalTime seconds...
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                // set layer weight so if we die in the air 
                // it doesnt play landing animation..
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
            }
        }
    }
}
