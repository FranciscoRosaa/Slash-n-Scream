using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    public void StartGame()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        //Debug.Log("QUIT");
        Application.Quit();
    }
}
