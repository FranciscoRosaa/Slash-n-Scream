using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotorietyUI : MonoBehaviour
{
    [SerializeField] private Image[] phaseImages;

    [Header("Phase Thresholds")]
    [SerializeField] private float phase2Threshold = 0.33f;
    [SerializeField] private float phase3Threshold = 0.90f;

    [Header("Warning Flash")]
    [SerializeField] private float flashThreshold = 0.90f;
    [SerializeField] private float flashSpeed = 3f;

    private float flashTimer = 0f;

    void Update()
    {
        if (NotorietyManager.Instance == null) return;

        float normalized = NotorietyManager.Instance.NotorietyNormalized;

        if (phaseImages != null && phaseImages.Length == 3)
        {
            phaseImages[0].gameObject.SetActive(normalized < phase2Threshold);
            phaseImages[1].gameObject.SetActive(normalized >= phase2Threshold && normalized < phase3Threshold);
            phaseImages[2].gameObject.SetActive(normalized >= phase3Threshold);
        }

        if (normalized >= phase3Threshold && phaseImages != null && phaseImages.Length == 3)
        {
            flashTimer += Time.deltaTime * flashSpeed;
            float alpha = (Mathf.Sin(flashTimer) + 1f) * 0.5f;

            Color c = phaseImages[2].color;
            c.a = Mathf.Lerp(0.4f, 1f, alpha);
            phaseImages[2].color = c;
        }
        else
        {
            flashTimer = 0f;
        }
    }
}
