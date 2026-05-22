using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image[] phaseImages;

    Player player;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
            if (player == null)
                return;
        }

        int health = player.GetHealth();

        for (int i = 0; i < phaseImages.Length; i++)
        {
            if (health == 3)
            {
                phaseImages[0].gameObject.SetActive(true);
                i++;
            }
            else if (health == 2)
            {
                phaseImages[0].gameObject.SetActive(false);
                phaseImages[1].gameObject.SetActive(true);
            }
            else if (health == 1)
            {
                phaseImages[1].gameObject.SetActive(false);
                phaseImages[2].gameObject.SetActive(true);
            }
        }
    }
}