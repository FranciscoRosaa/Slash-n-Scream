using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float      speed           = 5;
    [SerializeField] private float      jumpSpeed       = 300;
    [SerializeField] private float      maxJumpTime     = 0.1f;
    [SerializeField] private float      gravityOnJump   = 0.75f;
    [SerializeField] private float      gravityOnFall   = 1.0f;
    [SerializeField] private float      knockbackSpeed  = 100.0f;
    [SerializeField] private int        maxHealth       = 3;
    [SerializeField] private Transform  groundCheck;
    [SerializeField] private float      groundCheckRadius;
    [SerializeField] private LayerMask  groundLayer;
    [SerializeField] private Collider2D airCollider;
    [SerializeField] private Collider2D groundCollider;

    private Animator        animator;
    private SpriteRenderer  spriteRenderer;
    private Rigidbody2D     rb;
    private int             health;
    private bool            onGround;
    private float           horizontalAxis;
    private float           jumpTime;
    private float           knockbackTime;
    private float           invulnerabilityTime;
    private float           blinkTime;

    bool isKnockback    => knockbackTime > 0;
    bool isInvulnerable => invulnerabilityTime > 0;

    void Start()
    {
        animator       = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb             = GetComponent<Rigidbody2D>();
        health         = PlayerHealth.current > 0 ? PlayerHealth.current : maxHealth;
    }

    void Update()
    {
        // timers
        knockbackTime       -= Time.deltaTime;
        invulnerabilityTime -= Time.deltaTime;

        // blink while invulnerable
        if (isInvulnerable)
        {
            blinkTime -= Time.deltaTime;
            if (blinkTime <= 0)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                blinkTime = 0.1f;
            }
        }
        else
        {
            spriteRenderer.enabled = true;
        }

        // ground
        onGround               = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        airCollider.enabled    = !onGround;
        groundCollider.enabled = onGround;

        if (!isKnockback)
        {
            horizontalAxis = Input.GetAxis("Horizontal");

            // jump
            Vector2 vel = rb.linearVelocity;
            if (Input.GetButtonDown("Jump") && onGround)
            {
                vel.y           = jumpSpeed;
                jumpTime        = Time.time;
                rb.gravityScale = gravityOnJump;
            }
            else if (Input.GetButton("Jump") && (Time.time - jumpTime) < maxJumpTime)
            {
                rb.gravityScale = gravityOnJump;
            }
            else
            {
                rb.gravityScale = gravityOnFall;
            }
            rb.linearVelocity = vel;

            // flip sprite
            if      (horizontalAxis < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (horizontalAxis > 0) transform.rotation = Quaternion.identity;
        }

        // animator
        animator.SetBool("OnGround", onGround);
        animator.SetFloat("AbsVelocityX", Mathf.Abs(horizontalAxis * speed));
        animator.SetFloat("VelocityY", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        if (!isKnockback)
            rb.linearVelocity = new Vector2(horizontalAxis * speed, rb.linearVelocityY);
    }

    public void DealDamage(int value, float dirX)
    {
        if (isInvulnerable) return;

        health -= value;
        PlayerHealth.current = health;

        if (health <= 0)
        {
            PlayerHealth.current = 0;
            enabled              = false;
            rb.linearVelocity    = Vector2.zero;
            Invoke("GameOver", 1.0f);
            return;
        }

        rb.linearVelocity   = new Vector2(Mathf.Sign(dirX) * knockbackSpeed, knockbackSpeed);
        knockbackTime       = 0.4f;
        invulnerabilityTime = 2.0f;
    }

    void GameOver() => SceneManager.LoadScene("Game Over");

    public int GetHealth() => health;

    void OnDrawGizmos()
    {
        if (groundCheck)
        {
            Gizmos.color = onGround ? Color.yellow : Color.red;
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
