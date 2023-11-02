using UnityEngine;

public class Countdown : MonoBehaviour
{
    private bool is15Seconds = false;
    private float seconds;

    public void StartTimer()
    {
        seconds = 0f;
        is15Seconds = false;
    }

    private void Update()
    {
        seconds += Time.deltaTime;
        if (seconds >= 15f)
        {
            is15Seconds = true;
        }
    }

    public bool Is15Seconds()
    {
        return is15Seconds;
    }
}
