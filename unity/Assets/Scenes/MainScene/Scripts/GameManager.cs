using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Target target;
    [SerializeField] private Attack attack;
    [SerializeField] private Round roundManager;
    [SerializeField] private ShakeCamera shakeCamera;
    [SerializeField] private Timer timer;
    [SerializeField] private Scene scene;

    public static int Round;
    private string element;

    private readonly List<GameObject> hpList = new();
    private int beforeHp = 3;
    private int currentHp = 3;

    public static Dictionary<string, string[,]> Magics;

    private void Start()
    {
        Init();

        Magics = new Dictionary<string, string[,]>
        {
            { "Fire", new [,] { { "Left", "1" }, { "Top", "2" }, { "Bottom", "3" }, {"Right", "4"} } },
            { "Aqua", new [,] { { "Top", "1" }, { "Bottom", "2" }, { "Left", "3" }, { "Right", "4" } } },
            { "Wind", new [,] { { "Top", "1" }, { "Left", "2" }, { "Bottom", "3" }, { "Right", "4" }, {"Top2", "5"} } },
            { "Lightning", new [,] { { "Left", "1" }, { "Right", "2" }, { "BottomLeft", "3" }, { "Top", "4" }, { "BottomRight", "5" }, {"Left2", "6"}} },
            { "Free", new [,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
        };
    }

    private static async void Init()
    {
        await Task.Delay(500);
    }

    public void GameStart()
    {
        timer.TimerReset();
        Rounds();
        Hp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UDPManager.SendReset();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            DecreaseHp();
        }
    }

    public void OnHit(string hitTargetName)
    {
        var result = roundManager.Rounds(element, hitTargetName);
        if (result == null) return;
        attack.Type(result);
        Rounds();
    }

    public static void IncrementCount()
    {
        global::Round.Count++;
    }

    private void Hp()
    {
        if (currentHp == beforeHp) return;
        else if (currentHp == 0)
        {
            scene.GameOver();
            return;
        }

        var hp = GameObject.Find("HP" + beforeHp);
        hpList.Add(hp);
        hp.SetActive(false);

        beforeHp = currentHp;
    }

    private void DecreaseHp()
    {
        currentHp--;
        shakeCamera.CameraShaker();

        Hp();
    }

    private void Rounds()
    {
        Target.DestroyTargets();

        switch (++Round)
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
                scene.GameClear();
                break;
        }
    }

    public void Reset()
    {
        Round = 0;
        currentHp = 3;
        beforeHp = 3;
        timer.TimerReset();

        foreach (var hp in hpList)
        {
            hp.SetActive(true);
        }
    }
}
