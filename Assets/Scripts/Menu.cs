using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    public void StartGame()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
            GameManager.Instance.ResetAll();

        if (NotorietyManager.Instance != null)
            NotorietyManager.Instance.ResetNotoriety();

        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}