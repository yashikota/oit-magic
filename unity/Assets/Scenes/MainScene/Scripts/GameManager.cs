using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Target target;
    [SerializeField] private UDPManager udpManager;

    private int round = 0;
    private int count = 1;
    Dictionary<string, string[,]> rounds;
    List<string> checkedList;

    void Start()
    {
        // rounds
        rounds = new Dictionary<string, string[,]>
        {
            { "Round1", new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Right", "3" } } },
            { "Round2", new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Left", "3" }, { "Right", "4" } } },
            { "Round3", new string[,] { { "Top", "1" }, { "Left", "2" }, { "Bottom", "3" }, { "Right", "4" }, {"Top2", "5"} } },
            { "Round4", new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
            { "Round5", new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
            { "Round6", new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
            { "Round7", new string[,] { { "Left", "1" }, { "Right", "2" }, { "BottomLeft", "3" }, { "Top", "4" }, { "BottomRight", "5" }, {"Left2", "6"}} }
        };
        checkedList = new List<string>();

        Init();
    }

    private async void Init()
    {
        await Task.Delay(1000);
        Rounds(); // round 1
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            udpManager.SendReset();
        }
    }

    public async void OnHit(string hitTargetName)
    {
        // target is not selected
        if (!checkedList.Any())
        {
            count = 1;
        }

        string firstTargetName = rounds["Round" + round][0, 0];
        string currentTargetName = rounds["Round" + round][count - 1, 0].Replace("2", "");
        int targetLength = rounds["Round" + round].GetLength(0);
        string lastTargetNumber = rounds["Round" + round][targetLength - 1, 1];

        // start and end are same
        if (targetLength == count + 1 && (round == 3 || round == 7))
        {
            checkedList.Remove(firstTargetName);
            target.ChangeColor(firstTargetName, Color.white);
            target.ChangeText(firstTargetName, lastTargetNumber);
        }

        // already selected
        if (checkedList.Contains(hitTargetName))
        {
            count--;
            return;
        }

        if (hitTargetName == currentTargetName)
        {
            target.ChangeColor(hitTargetName, Color.blue);
            checkedList.Add(hitTargetName);

            // next round
            if (count == targetLength)
            {
                count = 1;
                await Task.Delay(1000);
                checkedList.Clear();
                Rounds();
            }
        }
        else
        {
            target.ChangeColor(hitTargetName, Color.red);
            count = 1;
            checkedList.Clear();

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
        target.DestroyTargets();

        switch (++round)
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
    }
}
