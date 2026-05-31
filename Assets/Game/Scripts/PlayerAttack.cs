using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private int knives = 1;

    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (knives <= 0) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null) continue;

            Vector2 toPlayer = transform.position - enemy.transform.position;
            float dot = Vector2.Dot(enemy.transform.right, toPlayer);

            if (dot < 0)
            {
                enemy.Die();
                if (GameManager.Instance != null)
                    GameManager.Instance.score++;
            }
            else
            {
                float dirX = transform.position.x - enemy.transform.position.x;
                player.DealDamage(1, dirX);
                enemy.AlertFlee();
            }

            knives--;
            return;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
