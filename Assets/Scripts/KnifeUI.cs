using UnityEngine;
using UnityEngine.UI;

public class KnifeUI : MonoBehaviour
{
    [SerializeField] private Image knifeImage;

    private PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = FindFirstObjectByType<PlayerAttack>();
    }

    void Update()
    {
        if (playerAttack == null) return;

        knifeImage.gameObject.SetActive(playerAttack.GetKnives() > 0);
    }
}
