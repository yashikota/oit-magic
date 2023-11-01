using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayLogData
{
    public float playTime;
    public bool isGameOver;
    public int round;
    public float time;
    public string playerName;
}

public class PlayLog : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Timer timer;

    public void Save()
    {
        float playTime = Time.timeSinceLevelLoad;
        bool isGameOver = gameManager.IsGameOver();
        int round = gameManager.GetRound();
        float time = timer.GetTime();
        string playerName = ""; // todo

        string path = Application.dataPath + "/playlog.json";
        string json = "";
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }

        PlayLogData data = new()
        {
            playTime = playTime,
            isGameOver = isGameOver,
            round = round,
            time = time,
            playerName = playerName
        };
        json += JsonUtility.ToJson(data) + "\n";
        File.WriteAllText(path, json);
    }
}
