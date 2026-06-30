using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float patrolSpeed = 1.0f;
    [SerializeField] private float minTurnTime = 1.0f;
    [SerializeField] private float maxTurnTime = 3.0f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckRadius = 0.1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform platformCheck;
    [SerializeField] private float platformCheckRadius = 0.1f;
    [SerializeField] private LayerMask platformLayer;

    [Header("Vision")]
    [SerializeField] private float visionRange = 3.5f;
    [SerializeField] private float visionAngle = 47.0f;
    [SerializeField] private LayerMask sightBlockLayer;

    [Header("Frozen")]
    [SerializeField] private float frozenDuration = 1.0f;

    [Header("Flee")]
    [SerializeField] private float fleeSpeed = 2.0f;
    [SerializeField] private float calmDownTime = 3.0f;

    [Header("Notoriety")]
    [SerializeField] private float notorietyOnSpot = 15f;
    [SerializeField] private float notorietyOnFrontAttack = 20f;
    [SerializeField] private float notorietyPerSecondVisible = 10.0f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    public enum State { Patrol, Frozen, Flee }

    private Rigidbody2D rb;
    private State state = State.Patrol;
    private float dir = 1.0f;
    private float flipCooldown = 0.0f;
    private float frozenTimer = 0.0f;
    private float turnTimer = 0.0f;
    private float calmDownTimer = 0.0f;
    private Player player;
    private bool wasAddingNotoriety = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        turnTimer = Random.Range(minTurnTime, maxTurnTime);
    }

    void Update()
    {
        if (player == null) return;

        switch (state)
        {
            case State.Patrol:
                Move(patrolSpeed);

                flipCooldown -= Time.deltaTime;
                if (flipCooldown <= 0 && (FindWall() || !FindPlatform()))
                {
                    dir = -dir;
                    flipCooldown = 0.5f;
                    turnTimer = Random.Range(minTurnTime, maxTurnTime);
                }

                turnTimer -= Time.deltaTime;
                if (turnTimer <= 0)
                {
                    dir = -dir;
                    turnTimer = Random.Range(minTurnTime, maxTurnTime);
                }

                if (CanSeePlayer())
                {
                    state = State.Frozen;
                    frozenTimer = frozenDuration;
                    rb.linearVelocity = Vector2.zero;

                    if (NotorietyManager.Instance != null)
                        NotorietyManager.Instance.AddNotoriety(notorietyOnSpot);
                }
                break;

            case State.Frozen:
                FacePlayer();
                rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
                frozenTimer -= Time.deltaTime;
                if (frozenTimer <= 0)
                    state = State.Flee;
                break;

            case State.Flee:
                FacePlayer();
                FleeBackwards();

                if (CanSeePlayer())
                {
                    if (NotorietyManager.Instance != null)
                        NotorietyManager.Instance.AddNotoriety(notorietyPerSecondVisible * Time.deltaTime);
                    wasAddingNotoriety = true;
                }
                else
                {
                    wasAddingNotoriety = false;
                }

                float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
                if (distToPlayer > visionRange)
                {
                    calmDownTimer += Time.deltaTime;
                    if (calmDownTimer >= calmDownTime)
                    {
                        state = State.Patrol;
                        calmDownTimer = 0.0f;
                        turnTimer = Random.Range(minTurnTime, maxTurnTime);
                    }
                }
                else
                {
                    calmDownTimer = 0.0f;
                }
                break;
        }

        if (state == State.Patrol)
        {
            if (dir < 0 && transform.right.x > 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (dir > 0 && transform.right.x < 0) transform.rotation = Quaternion.identity;
        }

        HandleFootsteps();
    }

    void FacePlayer()
    {
        float playerDir = player.transform.position.x - transform.position.x;
        if (playerDir < 0 && transform.right.x > 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (playerDir > 0 && transform.right.x < 0) transform.rotation = Quaternion.identity;
    }

    void FleeBackwards()
    {
        float fleeDir = Mathf.Sign(transform.position.x - player.transform.position.x);
        rb.linearVelocity = new Vector2(fleeDir * fleeSpeed, rb.linearVelocityY);
    }

    void Move(float speed)
    {
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocityY);
    }

    private void HandleFootsteps()
    {
        if (audioSource == null) return;

        if ((state == State.Patrol || state == State.Flee) && Mathf.Abs(rb.linearVelocity.x) > 0.05f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    public void PlayFootstepSoundAnimationEvent()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public bool CanSeePlayer()
    {
        Vector2 toPlayer = player.transform.position - transform.position;
        if (toPlayer.magnitude > visionRange) return false;
        if (Vector2.Angle(transform.right, toPlayer) > visionAngle) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, sightBlockLayer);
        return hit.collider == null;
    }

    bool FindWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer) != null;
    }

    bool FindPlatform()
    {
        return Physics2D.OverlapCircle(platformCheck.position, platformCheckRadius, platformLayer) != null;
    }

    public void AlertFlee()
    {
        state = State.Flee;
        rb.linearVelocity = Vector2.zero;
        if (NotorietyManager.Instance != null)
            NotorietyManager.Instance.AddNotoriety(notorietyOnFrontAttack);
    }

    public bool IsFrozen() => state == State.Frozen;
    public State GetState() => state;

    public void Die()
    {
        EnemyTracker.Instance?.OnEnemyDied();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (wallCheck) { Gizmos.color = Color.magenta; Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius); }
        if (platformCheck) { Gizmos.color = Color.cyan; Gizmos.DrawWireSphere(platformCheck.position, platformCheckRadius); }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, visionAngle) * transform.right * visionRange);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, -visionAngle) * transform.right * visionRange);
    }
}