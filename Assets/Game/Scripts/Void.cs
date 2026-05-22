using UnityEngine;

public class Void : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log($"Collided with {collider.name}");

        Player player = collider.GetComponentInParent<Player>();
        if (player != null)
        {
            float deltaX = player.transform.position.x - transform.position.x;
            player.DealDamage(3, deltaX);
        }
    }
}
