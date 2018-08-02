using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// delegate to pass an function
// so we can create an event...
public delegate void DeadEventHandler();

public class Player : Character
{
    [SerializeField]
    public Stat energy;

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

    public AudioClip slash_miss01;
    public AudioClip slash_miss02;
    public AudioClip slash_miss03;

    public AudioClip youdied;

    public AudioClip jump;

    public AudioClip run;

    public AudioClip die;

    public AudioClip pickup;

    public AudioClip whisper;

    public AudioClip powerJump;
    public AudioClip soulThrow;


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
    private ParticleSystem defendLeft;

    [SerializeField]
    private ParticleSystem defendRight;

    [SerializeField]
    private ParticleSystem rightSlash;

    [SerializeField]
    private ParticleSystem leftSlash;

    [SerializeField]
    private ParticleSystem jumperAnim;

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

    [SerializeField]
    private GameObject soulFireprefab;

    [SerializeField]
    private GameObject soulFireprefabLeft;

    public Rigidbody2D MyRigidbody { get; set; }

    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    private bool immortal = false;

    [SerializeField]
    private float immortalTime;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Canvas endgameCanvas;

    [SerializeField]
    private Canvas wingameCanvas;

    [SerializeField]
    private Text task4_status;
    private int task4_counter = 0;

    [SerializeField]
    private Text task4_cross;

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

    // 
    private float attackRate = 0.35f;
    private float nextAttack;

    // performing supajump
    private bool superJump = false;

    // firedSoul
    public static bool firedSoul = false;

    public static bool isDefending = false;

    public static int completedQuests;

    // Use this for initialization
    public override void Start()
    {
        // reset quests
        task4_counter = 0;
        task4_cross.text = "";
        completedQuests = 0;

        UnityEngine.Cursor.visible = false;
        // reset values, hide death canvas
        endgameCanvas.enabled = false;
        wingameCanvas.enabled = false;
        life = 3;
        IsDead = false;
        Time.timeScale = 1f;

        leftSlash.Stop();
        rightSlash.Stop();
        jumperAnim.Stop();
        defendRight.Stop();
        defendLeft.Stop();

        sounds = GetComponents<AudioSource>();
        audioSrc = sounds[0];
        footSrc = sounds[1];
        takehitSrc = sounds[2];
        whisperSrc = sounds[3];
        consumeSoul = sounds[4];

        // call Start function from Character
        base.Start();

        energy.Initialize();

        MyRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        FloatingTextController.Initialize();

        StartCoroutine(restoreEnergy());
    }

    IEnumerator restoreEnergy()
    {

        while (true)
        { // loops forever...

            if (energy.CurrentValue <= energy.MaxValue && (Attack || TakingDamage))
            { // if health < 100...
                energy.CurrentValue += 1; // increase health and wait the specified time
                yield return new WaitForSeconds(0.4f);
            }
            else if(energy.CurrentValue <= energy.MaxValue && !Attack && !TakingDamage)
            {
                energy.CurrentValue += 1; // increase health and wait the specified time
                yield return new WaitForSeconds(0.05f);
            }
            else
            { // if health >= 100, just yield 
                yield return null;
            }
        }
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

        if (!Input.GetKey(KeyCode.C))
        {
            defendRight.Stop();
            defendLeft.Stop();
            MyAnimator.SetBool("isdefending", false);
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

        if(completedQuests == 4)
        {
            UnityEngine.Cursor.visible = true;

            Time.timeScale = 0f;

            // turn on wingame canvas
            wingameCanvas.enabled = true;

            // turn on win sound            
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

        // save last position
        if (OnGround && !Jump && !superJump && !Attack && !isDefending && !TakingDamage)
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

        if (Jump && MyRigidbody.velocity.y == 0 && superJump)
        {
                jumperAnim.Play();
                MyRigidbody.AddForce(new Vector2(0, JumpForce * 1.7f));
                superJump = false;
                audioSrc.PlayOneShot(powerJump);
        }
        else if (Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, JumpForce));
            audioSrc.PlayOneShot(jump);
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time > nextAttack && energy.CurrentValue >= 10)
        {
            energy.CurrentValue -= Random.Range(10, 30);
            MyAnimator.SetTrigger("attack");

            nextAttack = Time.time + attackRate;

            if (facingRight)
            {
                rightSlash.Play();
            }
            else if (!facingRight)
            {
                leftSlash.Play();
            }

            int random = Random.Range(1, 3);
            if (random == 1) audioSrc.PlayOneShot(Player.Instance.slash_miss01);
            else if (random == 2) audioSrc.PlayOneShot(Player.Instance.slash_miss02);
            else if (random == 3) audioSrc.PlayOneShot(Player.Instance.slash_miss03);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MyAnimator.SetTrigger("jump");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpendSoul();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (energy.CurrentValue >= 48 && !Jump && OnGround)
            {
                energy.CurrentValue -= 48;
                MyAnimator.SetTrigger("jump");
                superJump = true;
            }
            else if (energy.CurrentValue <= 47)
            {
                warnText.text = "! ! NOT ENOUGH ENERGY ! !";
                Invoke("HideWarnText", 1.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(energy.CurrentValue >= 100 && GameManager.Instance.CollectedSouls > 1)
            {
                if (task4_counter < 1)
                {
                    task4_counter++;
                    task4_status.text = task4_counter + "/1";
                }

                if (task4_counter == 1)
                {
                    Player.completedQuests++;
                    task4_cross.text = "____________________________";
                }

                audioSrc.PlayOneShot(soulThrow);
                MyAnimator.SetTrigger("soulfire");
            }
            else if(energy.CurrentValue <= 100)
            {
                warnText.text = "! ! NOT ENOUGH ENERGY ! !";
                Invoke("HideWarnText", 1.5f);
            }
            else if(GameManager.Instance.CollectedSouls <= 1)
            {
                warnText.text = "! ! NOT ENOUGH SOULS(2 NEEDED) ! !";
                Invoke("HideWarnText", 1.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(energy.CurrentValue > 1)
            {
                if (facingRight) defendRight.Play();
                else if (!facingRight) defendLeft.Play();

                MyAnimator.SetBool("isdefending", true);
            }
            else
            {
                isDefending = false;
                MyAnimator.SetBool("isdefending", false);
            }
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
            int damage = 0;
            if (isDefending)
            {
                damage = Random.Range(1, 3);
            }
            else if(!isDefending && !immortal)
            {
                damage = Random.Range(1, 35);
                healthStat.CurrentValue -= damage;
            }

            // Debug.Log("i got damage :(");

            // if we are not dead
            if (!IsDead)
            {
                FloatingTextController.CreateFloatingText(damage.ToString(), transform);

                if (!isDefending)
                {
                    int random = Random.Range(1, 3);
                    if (random == 1) audioSrc.PlayOneShot(hurt01);
                    else if (random == 2) audioSrc.PlayOneShot(hurt02);
                    else if (random == 3) audioSrc.PlayOneShot(hurt03);

                    MyAnimator.SetTrigger("damage");
                    immortal = true;

                    StartCoroutine(IndicateImmortal());
                }

                // wait immortalTime seconds...
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                FloatingTextController.CreateFloatingText(damage.ToString(), transform);

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
        if (life > 0)
        {
            // play die sound
            audioSrc.PlayOneShot(die);
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
        else if (life <= 0)
        {
            audioSrc.PlayOneShot(youdied);
            UnityEngine.Cursor.visible = true;
            endgameCanvas.enabled = true;
            // Time.timeScale = 0f;
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

        if (other.gameObject.tag == "Soul")
        {
            // Debug.Log("playing");
            whisperSrc.PlayOneShot(whisper);
        }
    }

    public void SpendSoul()
    {
        if (GameManager.Instance.CollectedSouls > 0 && healthStat.CurrentValue < healthStat.MaxValue)
        {
            isHealing = true;
            int healthAmount = Random.Range(10, 15);

            healthStat.CurrentValue += healthAmount;

            FloatingTextController.CreateFloatingText(healthAmount.ToString(), transform);

            GameManager.Instance.CollectedSouls--;

            // play sound
            consumeSoul.PlayOneShot(consumeSoulClip);
        }
        else if (GameManager.Instance.CollectedSouls > 0 && healthStat.CurrentValue >= healthStat.MaxValue)
        {
            healthMaxText.text = "! ! YOU HAVE MAX HEALTH ! !";
            Invoke("HideHealthText", 1.5f);
        }
        else if (GameManager.Instance.CollectedSouls <= 0)
        {
            warnText.text = "! ! NO SOUL TO CONSUME ! !";
            Invoke("HideWarnText", 1.5f);
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

    public void ThrowSoul(int value)
    {
        if (OnGround && value == 0 && energy.CurrentValue >= 100)
        {
            // decrease souls amount
            GameManager.Instance.CollectedSouls -= 2;
            energy.CurrentValue -= 100;
            firedSoul = true;

            if (facingRight)
            {
                GameObject tmp = (GameObject)Instantiate(soulFireprefab, transform.position, Quaternion.identity);
                tmp.GetComponent<SoulFire>().Initialize(Vector2.right);
            }
            else
            {
                GameObject tmp = (GameObject)Instantiate(soulFireprefabLeft, transform.position, transform.rotation);
                tmp.GetComponent<SoulFire>().Initialize(Vector2.left);
            }
        }
    }
}
