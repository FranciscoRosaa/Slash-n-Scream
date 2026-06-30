using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [Header("Persistent State")]
    public int health;
    public int score;
    public float notoriety;

    void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void ResetAll()
    {
        health = 0;
        score = 0;
        notoriety = 0f;
    }

    public void ResetForRestart()
    {
        health = 0;
        notoriety = 0f;
    }
}
