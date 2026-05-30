using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void ExitGame()
    {
        //Debug.Log("QUIT");
        Application.Quit();
    }
}
