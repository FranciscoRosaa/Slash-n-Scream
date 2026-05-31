using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private int nextSceneIndex;
    [SerializeField] private GameObject doorVisual;

    void Start()
    {
        if (doorVisual != null)
            doorVisual.SetActive(false);
    }

    void Update()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        if (enemies.Length == 0)
        {
            if (doorVisual != null)
                doorVisual.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>() != null)
        {
            Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            if (enemies.Length == 0)
                SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
