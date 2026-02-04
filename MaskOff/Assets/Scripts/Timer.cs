using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Settings")]
    [SerializeField] private float startTime = 600f;

    private float currentTime;
    private bool isRunning = true;

    private void Start()
    {
        currentTime = startTime;
    }

    private void Update()
    {
        if (!isRunning || timerText == null) return;

        currentTime -= Time.deltaTime;

        timerText.text = FormatTime(currentTime);
    }

    private string FormatTime(float seconds)
    {
        if (seconds >= 0f)
        {
            int totalSecs = Mathf.FloorToInt(seconds);
            int minutes = totalSecs / 60;
            int secs = totalSecs % 60;
            return string.Format("Time Left: " + "{0}:{1:00}", minutes, secs);
        }

        // Negative: display as -0:01, -0:02, ...
        int absTotalSecs = Mathf.FloorToInt(Mathf.Abs(seconds));
        int min = absTotalSecs / 60;
        int s = absTotalSecs % 60;
        return string.Format("Time Left: " +"-{0}:{1:00}", min, s);
    }

    public void Pause() => isRunning = false;
    public void Resume() => isRunning = true;
    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
    }
}
