using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Target target;
    [SerializeField] private Attack attack;
    [SerializeField] private RoundManager roundManager;

    private int round = 0;
    private int count = 1;
    private string element;
    private List<string> checkedList;

    public static Dictionary<string, string[,]> Magics;
    public static Dictionary<string, bool> Elements;

    private void Start()
    {
        Init();

        checkedList = new List<string>();

        Magics = new Dictionary<string, string[,]>
        {
            { "Fire", new string[,] { { "Left", "1" }, { "Top", "2" }, { "Bottom", "3" }, {"Right", "4"} } },
            { "Aqua", new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Left", "3" }, { "Right", "4" } } },
            { "Wind", new string[,] { { "Top", "1" }, { "Left", "2" }, { "Bottom", "3" }, { "Right", "4" }, {"Top2", "5"} } },
            { "Lightning", new string[,] { { "Left", "1" }, { "Right", "2" }, { "BottomLeft", "3" }, { "Top", "4" }, { "BottomRight", "5" }, {"Left2", "6"}} },
            { "Free", new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
        };

        Elements = new Dictionary<string, bool> {
            { "Fire", true },
            { "Aqua", true },
            { "Wind", true },
        };
    }

    public void Reset()
    {
        Elements["Fire"] = true;
        Elements["Aqua"] = true;
        Elements["Wind"] = true;
    }

    private async void Init()
    {
        await Task.Delay(500);
        Rounds(); // round 1
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UDPManager.SendReset();
        }
    }

    public async Task OnHitAsync(string hitTargetName)
    {
        var result = await roundManager.Round(element, hitTargetName, round, count, checkedList);

        switch (result)
        {
            case null:
                return;
            case "True":
                Target.ChangeColor(hitTargetName, Color.blue);
                checkedList.Add(hitTargetName);
                break;
            case "False":
                Target.ChangeColor(hitTargetName, Color.red);
                count = 1;
                checkedList.Clear();

                await Task.Delay(500);
                foreach (var key in Magics[element].Cast<string>().Where((v, i) => i % 2 == 0).Select(v => v.Replace("2", "")).Except(checkedList))
                {
                    Target.ChangeColor(key, Color.white);
                }

                Reset();
                break;
            default:
                // Fire, Aqua, Wind, Lightning
                attack.Type(result);
                count = 1;
                await Task.Delay(1000);
                checkedList.Clear();
                Rounds();
                break;
        }
    }

    public void IncrementCount()
    {
        count++;
    }

    private void Rounds()
    {
        Target.DestroyTargets();

        switch (++round)
        {
            case 1:
                element = "Fire";
                target.GenerateTargets(Magics[element]);
                break;
            case 2:
                element = "Aqua";
                target.GenerateTargets(Magics[element]);
                break;
            case 3:
                element = "Wind";
                target.GenerateTargets(Magics[element]);
                break;
            case 4:
            case 5:
            case 6:
                element = "Free";
                target.GenerateTargets(Magics[element]);
                break;
            case 7:
                element = "Lightning";
                target.GenerateTargets(Magics[element]);
                break;
            default:
                break;
        }
    }
}
