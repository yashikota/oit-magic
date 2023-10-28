using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private int minute;
    [SerializeField] private float seconds;

    private float oldSeconds;
    private TextMeshProUGUI timerText;

    void Start()
    {
        minute = 0;
        seconds = 0f;
        oldSeconds = 0f;
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
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
