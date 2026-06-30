using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    public void StartGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetAll();

        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
