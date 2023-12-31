using System;
using System.IO;
using UnityEngine;
using TMPro;

[Serializable]
public class RankingData
{
    public int first;
    public int second;
    public int third;
    public int fourth;
    public int fifth;
}

public class Ranking : MonoBehaviour
{
    private string path;
    private int[] ranking = new int[5];
    [SerializeField] private GameObject rankingBoard;

    private void Start()
    {
        const string fileName = "ranking.json";
        var userName = Environment.UserName;
        path = @"C:\Users\" + userName + @"\Documents\oit-magic";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path += @"\" + fileName;

        ranking = new[] { 5995, 5996, 5997, 5998, 5999 };

        LoadRanking();
    }

    private void LoadRanking()
    {
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var rankingData = JsonUtility.FromJson<RankingData>(json);

        ranking[0] = rankingData.first;
        ranking[1] = rankingData.second;
        ranking[2] = rankingData.third;
        ranking[3] = rankingData.fourth;
        ranking[4] = rankingData.fifth;

        ShowRanking();
    }

    public void UpdateRanking(int time)
    {
        var isUpdated = false;

        for (var i = 0; i < ranking.Length; i++)
        {
            if (time >= ranking[i] || isUpdated) continue;
            for (var j = ranking.Length - 1; j > i; j--)
            {
                ranking[j] = ranking[j - 1];
            }
            ranking[i] = time;
            isUpdated = true;
        }

        ShowRanking();
        SaveRanking();
    }

    private void ShowRanking()
    {
        for (var i = 0; i < ranking.Length; i++)
        {
            // convert
            var minute = ranking[i] / 60;
            var seconds = ranking[i] % 60;
            var timeString = minute.ToString("00") + " : " + seconds.ToString("00");

            // show
            rankingBoard.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = timeString;
        }
    }

    private void SaveRanking()
    {
        var rankingData = new RankingData()
        {
            first = ranking[0],
            second = ranking[1],
            third = ranking[2],
            fourth = ranking[3],
            fifth = ranking[4],
        };

        var json = JsonUtility.ToJson(rankingData);
        File.WriteAllText(path, json);
    }
}
