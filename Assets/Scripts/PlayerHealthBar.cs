using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image[] phaseImages;

    Player player;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        int health = player.GetHealth();

        phaseImages[0].gameObject.SetActive(health == 3);
        phaseImages[1].gameObject.SetActive(health == 2);
        phaseImages[2].gameObject.SetActive(health == 1);
    }
}