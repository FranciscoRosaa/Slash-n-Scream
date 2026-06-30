using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private int nextSceneIndex;
    [SerializeField] private GameObject doorVisual;

    private bool isOpen = false;

    void Start()
    {
        if (doorVisual != null)
            doorVisual.SetActive(false);

        EnemyTracker.OnAllEnemiesDead += OpenDoor;

        if (EnemyTracker.Instance != null && EnemyTracker.Instance.GetEnemyCount() == 0)
            OpenDoor();
    }

    void OnDestroy()
    {
        EnemyTracker.OnAllEnemiesDead -= OpenDoor;
    }

    void OpenDoor()
    {
        isOpen = true;
        if (doorVisual != null)
            doorVisual.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen) return;

        if (collision.GetComponentInParent<Player>() != null)
        {
            if (NotorietyManager.Instance != null)
                NotorietyManager.Instance.SaveToGameManager();

            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
