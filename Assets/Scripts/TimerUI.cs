using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    void Update()
    {
        if (LevelTimer.Instance == null) return;

        timerText.text = "Time: " + Mathf.CeilToInt(LevelTimer.Instance.GetMainTimer());
    }
}
