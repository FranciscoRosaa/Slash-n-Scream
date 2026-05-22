using UnityEngine;

public class Spikes : MonoBehaviour
{

    void Start()
    {
        
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
            player.DealDamage(1, deltaX);
        }
    }
}
