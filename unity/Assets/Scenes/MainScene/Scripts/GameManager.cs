using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Target target;
    [SerializeField] private Attack attack;
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private PlayLog playLog;

    private bool isTitle = true;
    private bool isGameOver = false;
    private bool isGameClear = false;

    public static int round = 0;
    private string element;

    private List<GameObject> hpList = new();
    private int beforeHP = 3;
    private int currentHP = 3;

    public static Dictionary<string, string[,]> Magics;

    private void Start()
    {
        Init();

        Magics = new Dictionary<string, string[,]>
        {
            { "Fire", new string[,] { { "Left", "1" }, { "Top", "2" }, { "Bottom", "3" }, {"Right", "4"} } },
            { "Aqua", new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Left", "3" }, { "Right", "4" } } },
            { "Wind", new string[,] { { "Top", "1" }, { "Left", "2" }, { "Bottom", "3" }, { "Right", "4" }, {"Top2", "5"} } },
            { "Lightning", new string[,] { { "Left", "1" }, { "Right", "2" }, { "BottomLeft", "3" }, { "Top", "4" }, { "BottomRight", "5" }, {"Left2", "6"}} },
            { "Free", new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
        };
    }

    private async void Init()
    {
        await Task.Delay(500);
        Rounds(); // round 1
    }

    public int GetRound()
    {
        return round;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UDPManager.SendReset();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            DecreaseHP();
        }

        HP();
    }

    public void OnHit(string hitTargetName)
    {
        var result = roundManager.Round(element, hitTargetName);
        if (result == null) return;
        else
        {
            attack.Type(result);
            Rounds();
        }
    }

    public void IncrementCount()
    {
        RoundManager.Count++;
    }

    private void HP()
    {
        if (currentHP == beforeHP) return;
        if (currentHP == 0) isGameOver = true;

        var hp = GameObject.Find("HP" + (currentHP + 1));
        hpList.Add(hp);
        hp.SetActive(false);

        beforeHP = currentHP;
    }

    private void DecreaseHP()
    {
        currentHP--;
    }

    public bool IsTitle()
    {
        return isTitle;
    }

    public void SetTitle(bool isTitle)
    {
        this.isTitle = isTitle;
    }

    public bool IsGameClear()
    {
        return isGameClear;
    }

    public void SetGameClear(bool isGameClear)
    {
        this.isGameClear = isGameClear;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void SetGameOver(bool isGameOver)
    {
        this.isGameOver = isGameOver;
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
                isGameClear = true;
                break;
        }
    }

    public void Reset()
    {
        round = 0;
        currentHP = 3;
        beforeHP = 3;
        isGameOver = false;
        isGameClear = false;

        foreach (var hp in hpList)
        {
            hp.SetActive(true);
        }
    }
}
