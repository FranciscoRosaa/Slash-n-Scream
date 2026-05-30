using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpSpeed = 300;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D airCollider;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private float maxJumpTime = 0.1f;
    [SerializeField] private float gravityOnJump = 0.75f;
    [SerializeField] private float gravityOnFall = 1.0f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackSpeed = 100.0f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool onGround;
    private int health;
    private float invulnerabilityTime;
    private float blinkTime;
    private float knockbackTime;

    private float jumpTime;
    private float horizontalAxis;

    bool isKnockback => (knockbackTime > 0.0f);
    bool isInvulnerable => (invulnerabilityTime > 0.0f);

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (!isKnockback)
        {
            Vector2 currentVelocity = rb.linearVelocity;

            currentVelocity.x = horizontalAxis * speed;

            rb.linearVelocity = currentVelocity;
        }
    }

    void Update()
    {
        if (isInvulnerable)
        {
            invulnerabilityTime -= Time.deltaTime;
            if (invulnerabilityTime <= 0.0f)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                blinkTime -= Time.deltaTime;
                if (blinkTime <= 0.0f)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                    blinkTime = 0.1f;
                }
            }
        }
        if (knockbackTime > 0.0f)
        {
            knockbackTime -= Time.deltaTime;
        }

        ComputeGround();

        airCollider.enabled = !onGround;
        groundCollider.enabled = onGround;

        Vector2 currentVelocity = rb.linearVelocity;

        if (!isKnockback)
        {
            horizontalAxis = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                if (onGround)
                {
                    currentVelocity.y = jumpSpeed;
                    jumpTime = Time.time;
                    rb.gravityScale = gravityOnJump;
                }
            }
            else if (Input.GetButton("Jump"))
            {
                if ((Time.time - jumpTime) < maxJumpTime)
                {
                    rb.gravityScale = gravityOnJump;
                }
                else
                {
                    rb.gravityScale = gravityOnFall;
                }
            }
            else
            {
                jumpTime = Time.time - maxJumpTime;
                rb.gravityScale = gravityOnFall;
            }

            rb.linearVelocity = currentVelocity;
        }

        animator.SetBool("OnGround", onGround);
        animator.SetFloat("AbsVelocityX", Mathf.Abs(horizontalAxis * speed));
        animator.SetFloat("VelocityY", currentVelocity.y);

        if ((horizontalAxis < 0) && (transform.right.x > 0))
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if ((horizontalAxis > 0) && (transform.right.x < 0))
            transform.rotation = Quaternion.identity;

        //if (onGround) spriteRenderer.color = Color.green;
        //else spriteRenderer.color = Color.red;
    }

    void ComputeGround()
    {
        onGround = false;

        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (collider != null)
            onGround = true;
    }

    public void DealDamage(int value, float dirX)
    {
        if (invulnerabilityTime > 0) return;

        health = health - value;
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        float speedX = Mathf.Sign(dirX) * knockbackSpeed;
        rb.linearVelocity = new Vector2(speedX, knockbackSpeed);
        knockbackTime = 0.4f;

        //Debug.Log($"Health = {health} // {speedX}");

        invulnerabilityTime = 2.0f;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck)
        {
            Gizmos.color = (onGround) ? (Color.yellow) : (Color.red);
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public int GetHealth()
    {
        return health;
    }
}
