using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1.0f;
    [SerializeField] private float stopPosition = 1.0f;
    [SerializeField] private float delayAfterStop = 3.0f;
    [SerializeField] private int nextSceneIndex = 0;

    private RectTransform rectTransform;
    private bool stopped = false;
    private float stopTimer = 0.0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!stopped)
        {
            if (rectTransform.anchoredPosition.y >= stopPosition)
            {
                stopped = true;
            }
            else
            {
                rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
            }
        }
        else
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= delayAfterStop)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
}