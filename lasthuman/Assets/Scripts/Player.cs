using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// delegate to pass an function
// so we can create an event...
public delegate void DeadEventHandler();

public class Player : Character
{
    // array of AudioSources
    public AudioSource[] sounds;
    public static AudioSource audioSrc;
    public static AudioSource footSrc;
    public static AudioSource takehitSrc;
    public static AudioSource whisperSrc;
    public static AudioSource consumeSoul;

    public AudioClip consumeSoulClip;

    public AudioClip hurt01;
    public AudioClip hurt02;
    public AudioClip hurt03;

    public AudioClip zombie_attack1;
    public AudioClip zombie_attack2;

    public AudioClip slash_miss01;
    public AudioClip slash_miss02;
    public AudioClip slash_miss03;

    public AudioClip jump;

    public AudioClip run;

    public AudioClip die;

    public AudioClip pickup;

    public AudioClip whisper;

    public static bool HitEnemy = false;

    // liczba zyc
    public static int life = 3;

    // event that enemy can listen to...
    // whenever player dies , triggers this
    // event and enemy knows that he is dead
    public event DeadEventHandler Dead;

    // variables start with small letter
    private static Player instance;

    // Warning text when no souls
    [SerializeField]
    private Text warnText;

    [SerializeField]
    private Text healthMaxText;

    // access life text
    [SerializeField]
    private Text lifesText;

    // bool healing 
    public static bool isHealing = false;

    [SerializeField]
    private ParticleSystem rightSlash;

    [SerializeField]
    private ParticleSystem leftSlash;

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
            if (healthStat.CurrentValue <= 0)
            {
                // make sure to trigger OnDead function
                OnDead();
            }

            return healthStat.CurrentValue <= 0;
        }

        set
        {

        }
    }
    // properties start with capital letter!!!
    private Vector2 startPos;

    // to turn off particle systems
    void OnAwake()
    {
        leftSlash.Stop();
        rightSlash.Stop();
    }

    // Use this for initialization
    public override void Start()
    {
        sounds = GetComponents<AudioSource>();
        audioSrc = sounds[0];
        footSrc = sounds[1];
        takehitSrc = sounds[2];
        whisperSrc = sounds[3];

        // call Start function from Character
        base.Start();

        MyRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        FloatingTextController.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TakingDamage && !IsDead)
        {
            if (transform.position.y <= -40f)
            {
                Death();
            }
            HandleInput();
        }

        // handle footstep sound effect
        if (!IsDead && OnGround && !Attack && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            footSrc.UnPause();
        }
        else
        {
            footSrc.Pause();
        }
    }

    void FixedUpdate()
    {
        // if we are not taking damage and we are not dead we can do things...
        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            OnGround = IsGrounded();

            HandleMovement(horizontal);
            Flip(horizontal);
            HandleLayers();
        }

        if (OnGround)
        {
            startPos = transform.position;
        }
    }

    public void OnDead()
    {
        if (Dead != null)
        {
            // triggering event called Dead
            Dead();
        }
    }
    private void HandleMovement(float horizontal)
    {
        if (MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }

        if (!Attack && (OnGround || aircontrol))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }

        if (Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, JumpForce));
            audioSrc.PlayOneShot(jump);
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            MyAnimator.SetTrigger("attack");
            if(facingRight)
            {
                rightSlash.Play();
            }
            else if(!facingRight)
            {
                leftSlash.Play();
            }
            int random = Random.Range(1, 3);
            if (random == 1) audioSrc.PlayOneShot(slash_miss01);
            else if (random == 2) audioSrc.PlayOneShot(slash_miss02);
            else if (random == 3) audioSrc.PlayOneShot(slash_miss03);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MyAnimator.SetTrigger("jump");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpendSoul();
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
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
        while (immortal)
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
        if (!immortal)
        {

            int damage = Random.Range(2, 19);
            healthStat.CurrentValue -= damage;

            // Debug.Log("i got damage :(");

            // if we are not dead
            if (!IsDead)
            {
                FloatingTextController.CreateFloatingText(damage.ToString(), transform);

                int random = Random.Range(1, 3);
                if (random == 1) audioSrc.PlayOneShot(hurt01);
                else if (random == 2) audioSrc.PlayOneShot(hurt02);
                else if (random == 3) audioSrc.PlayOneShot(hurt03);

                random = Random.Range(1, 2);
                if (random == 1) takehitSrc.PlayOneShot(zombie_attack1);
                else if (random == 2) takehitSrc.PlayOneShot(zombie_attack1);

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
                audioSrc.PlayOneShot(die);
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
            }
        }
    }

    public override void Death()
    {
        MyRigidbody.velocity = Vector2.zero;
        if(life > 0)
        {
            life -= 1;
        }

        lifesText.text = life.ToString();

        // go from death animation to idle..
        // we need to respawn
        if (life > 0)
        {
            MyAnimator.SetTrigger("idle");
            healthStat.CurrentValue = healthStat.MaxValue;

            // respawn the player in last position he was on ground - 10 
            transform.position = new Vector2(startPos.x - 10, startPos.y);
        }
    }

    // functionality to pickup souls
    private void OnCollisionEnter2D(Collision2D other)
    {
        // if I collide with a soul
        if (other.gameObject.tag == "Soul")
        {
            GameManager.Instance.CollectedSouls++;

            // play pick up sound
            audioSrc.PlayOneShot(pickup);

            // stop whispers sound effect
            whisperSrc.Stop();

            // destroy a soul
            Destroy(other.gameObject);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if(other.gameObject.tag == "Soul")
        {
            // Debug.Log("playing");
            whisperSrc.PlayOneShot(whisper);
        }
    }

    public void SpendSoul()
    {
        if(GameManager.Instance.CollectedSouls > 0 && healthStat.CurrentValue < healthStat.MaxValue)
        {
            isHealing = true;
            int healthAmount = Random.Range(10, 15);

            GameManager.Instance.CollectedSouls--;

            // play sound
            consumeSoul.PlayOneShot(consumeSoulClip);

            healthStat.CurrentValue += healthAmount;

            FloatingTextController.CreateFloatingText(healthAmount.ToString(), transform);
        }
        else if(GameManager.Instance.CollectedSouls > 0 && healthStat.CurrentValue >= 0)
        {
            healthMaxText.text = "! ! YOU HAVE MAX HEALTH ! !";
            Invoke("HideHealthText", 1.5f);
        }
        else if(GameManager.Instance.CollectedSouls <= 0)
        {
            warnText.text = "! ! NO SOUL TO CONSUME ! !";
            Invoke("HideText", 1.5f);
        }
    }

    private void HideWarnText()
    {
        warnText.text = "";
    }

    private void HideHealthText()
    {
        healthMaxText.text = "";
    }
}
