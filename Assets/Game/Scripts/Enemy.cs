using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float      speed = 150.0f;
    [SerializeField] private Transform  wallCheck;
    [SerializeField] private float      wallCheckRadius;
    [SerializeField] private LayerMask  wallLayer;
    [SerializeField] private Transform  platformCheck;
    [SerializeField] private float      platformCheckRadius;
    [SerializeField] private LayerMask  platformLayer;

    Rigidbody2D rb;
    float       dir = 1.0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        /*rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocityY);

        if (FindWall())
        {
            dir = -dir;
        }
        if (!FindPlatform())
        {
            dir = -dir;
        }

        if ((dir < 0) && (transform.right.x > 0))
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if ((dir > 0) && (transform.right.x < 0))
            transform.rotation = Quaternion.identity;*/
    }

    bool FindWall()
    {
        Collider2D collider = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
        if (collider != null)
            return true;

        return false;
    }

    bool FindPlatform()
    {
        Collider2D collider = Physics2D.OverlapCircle(platformCheck.position, platformCheckRadius, platformLayer);
        if (collider != null)
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log($"Collided with {collider.name}");

        Player player = collider.GetComponentInParent<Player>();
        if (player != null)
        {
            float deltaX = player.transform.position.x - transform.position.x;
            player.DealDamage(1, deltaX);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (wallCheck)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
        if (platformCheck)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(platformCheck.position, platformCheckRadius);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocityY);

        if (FindWall())
        {
            dir = -dir;
        }
        if (!FindPlatform())
        {
            dir = -dir;
        }

        if ((dir < 0) && (transform.right.x > 0))
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if ((dir > 0) && (transform.right.x < 0))
            transform.rotation = Quaternion.identity;
    }

    public void Die()
    {

        Destroy(gameObject);
    }

}
