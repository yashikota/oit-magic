using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private int minute;
    [SerializeField] private float seconds;

    private float oldSeconds;
    private TextMeshProUGUI timerText;
    private bool isStop = false;

    private void Start()
    {
        minute = 0;
        seconds = 0f;
        oldSeconds = 0f;
        timerText = GetComponent<TextMeshProUGUI>();
    }

    public void Stop()
    {
        isStop = true;
    }

    public void Reset()
    {
        minute = 0;
        seconds = 0f;
        oldSeconds = 0f;
        timerText.text = minute.ToString("00") + " : " + ((int)seconds).ToString("00");
    }

    public float GetTime()
    {
        return minute * 60 + seconds;
    }

    public void Update()
    {
        if (isStop) return;

        seconds += Time.deltaTime;
        if (seconds >= 60f)
        {
            minute++;
            seconds -= 60;
        }

        if ((int)seconds != (int)oldSeconds)
        {
            timerText.text = minute.ToString("00") + " : " + ((int)seconds).ToString("00");
        }
        oldSeconds = seconds;
    }
}
