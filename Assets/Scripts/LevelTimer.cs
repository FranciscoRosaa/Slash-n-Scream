using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float timeLimit = 60.0f;
    [SerializeField] private float comboCooldown = 5.0f;
    [SerializeField] private float bonusTime = 10.0f;

    private float mainTimer;
    private float comboTimer = 0.0f;
    private bool comboActive = false;

    public static LevelTimer Instance;

    void Awake()
    {
        Instance = this;
        mainTimer = timeLimit;
    }

    void Update()
    {
        mainTimer -= Time.deltaTime;
        if (mainTimer <= 0)
        {
            mainTimer = 0;
            if (GameManager.Instance != null) GameManager.Instance.health = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (comboActive)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                comboActive = false;
                comboTimer = 0;
            }
        }
    }

    public void OnVictimKilled()
    {
        if (comboActive)
        {
            mainTimer += bonusTime;
            comboActive = false;
            comboTimer = 0;
        }

        comboActive = true;
        comboTimer = comboCooldown;
    }

    public float GetMainTimer() { return mainTimer; }
    public float GetComboTimer() { return comboTimer; }
    public bool IsComboActive() { return comboActive; }
}