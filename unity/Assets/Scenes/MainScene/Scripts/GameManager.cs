using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Target target;

    private int round = 0;
    private int count = 0;
    Dictionary<string, string[,]> rounds;

    void Start()
    {
        target = FindObjectOfType<Target>();

        // rounds
        rounds = new Dictionary<string, string[,]>
        {
            { "Round1", new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Right", "3" } } },
            { "Round2", new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Left", "3" }, { "Right", "4" } } },
            { "Round3", new string[,] { { "Top", "1" }, { "Left", "2" }, { "Bottom", "3" }, { "Right", "4" } } },
            { "Round4", new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
            { "Round5", new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
            { "Round6", new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
            { "Round7", new string[,] { { "Left", "1" }, { "Right", "2" }, { "BottomLeft", "3" }, { "Top", "4" }, { "BottomRight", "5" }} }
        };

        Rounds();
    }

    public async void OnHit(string targetName)
    {
        if (targetName == rounds["Round" + round][count, 0])
        {
            target.ChangeColor(targetName, Color.blue);

            // next round
            if (count == rounds["Round" + round].GetLength(0) - 1)
            {
                count = 0;
                await Task.Delay(1000);
                Rounds();
            }
        }
        else
        {
            target.ChangeColor(targetName, Color.red);
            count = 0;

            await Task.Delay(500);
            foreach (string key in rounds["Round" + round])
            {
                target.ChangeColor(key, Color.white);
            }
        }
    }

    public void IncrementCount()
    {
        count++;
    }

    private void Rounds()
    {
        Debug.Log(round + 1);

        // destroy targets
        target.DestroyTargets();

        // generate targets
        switch (round + 1)
        {
            case 1:
                target.GenerateTargets(rounds["Round1"]);
                break;
            case 2:
                target.GenerateTargets(rounds["Round2"]);
                break;
            case 3:
                target.GenerateTargets(rounds["Round3"]);
                break;
            case 4:
            case 5:
            case 6:
                target.GenerateTargets(rounds["Round4"]);
                break;
            case 7:
                target.GenerateTargets(rounds["Round7"]);
                break;
            default:
                break;
        }
        round++;
    }
}
