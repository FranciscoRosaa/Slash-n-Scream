using UnityEngine;
using System;

public class EnemyTracker : MonoBehaviour
{
    public static EnemyTracker Instance { get; private set; }

    public static event Action OnAllEnemiesDead;

    private int enemyCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;

        if (enemyCount == 0)
            OnAllEnemiesDead?.Invoke();
    }

    public void OnEnemyDied()
    {
        enemyCount--;
        enemyCount = Mathf.Max(0, enemyCount);

        if (enemyCount == 0)
            OnAllEnemiesDead?.Invoke();
    }

    public int GetEnemyCount() => enemyCount;
}
