using UnityEngine;

public class KnifePickup : MonoBehaviour
{
    [SerializeField] private float respawnTime = 10.0f;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private float respawnTimer = 0.0f;
    private bool isPickedUp = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isPickedUp)
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= respawnTime)
            {
                spriteRenderer.enabled = true;
                col.enabled = true;
                isPickedUp = false;
                respawnTimer = 0.0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerAttack playerAttack = collision.GetComponentInParent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.AddKnife();

            spriteRenderer.enabled = false;
            col.enabled = false;
            isPickedUp = true;
        }
    }
}