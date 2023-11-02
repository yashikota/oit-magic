using System;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayData
{
    public string playTime;
    public int round;
    public string gameStatus;
    public string endTime;
}

public class PlayLog : MonoBehaviour
{
    public static bool IsGameOver = false;
    [SerializeField] private Timer timer;

    public void Save()
    {
        const string fileName = "log.json";
        var userName = Environment.UserName;
        var path = @"C:\Users\" + userName + @"\Documents\oit-magic";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path += @"\" + fileName;
        if (!File.Exists(path)) File.Create(path);

        var data = new PlayData()
        {
            playTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            round = GameManager.Round,
            gameStatus = IsGameOver ? "GameOver" : "GameClear",
            endTime = timer.GetTimeString(),
        };

        var json = JsonUtility.ToJson(data);
        File.AppendAllText(path, json + Environment.NewLine);
    }
}
