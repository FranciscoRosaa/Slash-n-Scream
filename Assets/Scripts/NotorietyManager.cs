using UnityEngine;
using UnityEngine.SceneManagement;

public class NotorietyManager : MonoBehaviour
{
    public static NotorietyManager Instance { get; private set; }

    [Header("Notoriety Settings")]
    [SerializeField] private float maxNotoriety = 100f;
    [SerializeField] private float notorietyDecayPerSecond = 2f;

    [Header("Police")]
    [SerializeField] private int gameOverSceneIndex = 0;
    [SerializeField] private float policeArrivalDelay = 2.0f;

    [Header("Difficulty Scaling")]
    [SerializeField] private float minSpeedMultiplier = 1.0f;
    [SerializeField] private float maxSpeedMultiplier = 2.0f;

    private float currentNotoriety = 0f;
    private bool policeArrived = false;
    private float policeTimer = 0f;

    public float Notoriety => currentNotoriety;
    public float NotorietyNormalized => currentNotoriety / maxNotoriety;
    public bool PoliceArrived => policeArrived;

    public float DifficultyMultiplier =>
        Mathf.Lerp(minSpeedMultiplier, maxSpeedMultiplier, NotorietyNormalized);

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (policeArrived)
        {
            policeTimer -= Time.deltaTime;
            if (policeTimer <= 0)
                SceneManager.LoadScene(gameOverSceneIndex);
            return;
        }

        currentNotoriety -= notorietyDecayPerSecond * Time.deltaTime;
        currentNotoriety = Mathf.Clamp(currentNotoriety, 0f, maxNotoriety);

        if (currentNotoriety >= maxNotoriety)
            TriggerPolice();
    }

    public void AddNotoriety(float amount)
    {
        if (policeArrived) return;
        currentNotoriety = Mathf.Min(currentNotoriety + amount, maxNotoriety);

        if (currentNotoriety >= maxNotoriety)
            TriggerPolice();
    }

    void TriggerPolice()
    {
        if (policeArrived) return;
        policeArrived = true;
        policeTimer = policeArrivalDelay;
    }

    public void SaveToGameManager()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.notoriety = currentNotoriety;
    }

    public void LoadFromGameManager()
    {
        if (GameManager.Instance != null)
            currentNotoriety = GameManager.Instance.notoriety;
    }
}
