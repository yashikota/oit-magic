using System;
using System.IO;
using UnityEngine;

[Serializable]
public class RankingData
{
    public string playTime;
    public int round;
    public string gameStatus;
    public string endTime;
}

public class PlayLog : MonoBehaviour
{
    public static bool isGameOver = false;
    [SerializeField] private Timer timer;

    public void Save()
    {
        string fileName = "log.json";
        string userName = Environment.UserName;
        string path = @"C:\Users\" + userName + @"\Documents\oit-magic";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path += @"\" + fileName;
        if (!File.Exists(path)) File.Create(path);

        var data = new RankingData()
        {
            playTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
            round = GameManager.round,
            gameStatus = isGameOver ? "GameOver" : "GameClear",
            endTime = timer.GetTimeString(),
        };

        var json = JsonUtility.ToJson(data);
        File.AppendAllText(path, json + Environment.NewLine);
    }
}
