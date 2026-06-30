using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private int knives = 1;
    [SerializeField] private int maxKnives = 1;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
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

        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

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
                if (LevelTimer.Instance != null)
                    LevelTimer.Instance.OnVictimKilled();
            }
            else
            {
                enemy.AlertFlee();
            }

            knives--;
            return;
        }
    }

    public int GetKnives()
    {
        return knives;
    }

    public void AddKnife()
    {
        knives = Mathf.Min(knives + 1, maxKnives);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}