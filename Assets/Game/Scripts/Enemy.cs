using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float patrolSpeed = 2.0f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckRadius = 0.1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform platformCheck;
    [SerializeField] private float platformCheckRadius = 0.1f;
    [SerializeField] private LayerMask platformLayer;

    [Header("Vision")]
    [SerializeField] private float visionRange = 5.0f;
    [SerializeField] private float visionAngle = 60.0f;
    [SerializeField] private LayerMask sightBlockLayer;

    [Header("Frozen")]
    [SerializeField] private float frozenDuration = 1.5f;

    [Header("Flee")]
    [SerializeField] private float fleeSpeed = 4.0f;

    public enum State { Patrol, Frozen, Flee }

    private Rigidbody2D rb;
    private State state = State.Patrol;
    private float dir = 1.0f;
    private float flipCooldown = 0.0f;
    private float frozenTimer = 0.0f;
    private Player player;

    void Awake() => rb = GetComponent<Rigidbody2D>();
    void Start() => player = FindFirstObjectByType<Player>();

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
                }

                if (CanSeePlayer())
                {
                    state = State.Frozen;
                    frozenTimer = frozenDuration;
                    rb.linearVelocity = Vector2.zero;
                }
                break;

            case State.Frozen:
                // stand still and count down
                rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
                frozenTimer -= Time.deltaTime;
                if (frozenTimer <= 0)
                    state = State.Flee;
                break;

            case State.Flee:
                dir = Mathf.Sign(transform.position.x - player.transform.position.x);
                Move(fleeSpeed);
                break;
        }

        if (dir < 0 && transform.right.x > 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (dir > 0 && transform.right.x < 0) transform.rotation = Quaternion.identity;
    }

    void Move(float speed) => rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocityY);

    bool CanSeePlayer()
    {
        Vector2 toPlayer = player.transform.position - transform.position;
        if (toPlayer.magnitude > visionRange) return false;
        if (Vector2.Angle(transform.right, toPlayer) > visionAngle) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, sightBlockLayer);
        return hit.collider == null;
    }

    bool FindWall() => Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer) != null;
    bool FindPlatform() => Physics2D.OverlapCircle(platformCheck.position, platformCheckRadius, platformLayer) != null;

    public bool IsFrozen() => state == State.Frozen;

    public void Die() => Destroy(gameObject);

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