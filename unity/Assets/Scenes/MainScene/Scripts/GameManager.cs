using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Target target;
    [SerializeField] private Attack attack;
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private ShakeCamera shakeCamera;
    [SerializeField] private Timer timer;
    [SerializeField] private Scene scene;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Battle battle;
    [SerializeField] private Particle particle;

    [SerializeField] private AudioClip[] clips;

    public static int Round;
    private string playerElement;

    private readonly List<GameObject> hpList = new();
    private int beforeHp = 3;
    private int currentHp = 3;

    private AudioSource audioSource;

    public static Dictionary<string, string[,]> Magics;

    private void Start()
    {
        InitDelay();

        audioSource = GetComponent<AudioSource>();

        Magics = new Dictionary<string, string[,]>
        {
            { "Fire", new [,] { { "Left", "1" }, { "Top", "2" }, { "Bottom", "3" }, { "Right", "4"} } },
            { "Aqua", new [,] { { "Top", "1" }, { "Bottom", "2" }, { "Left", "3" }, { "Right", "4" } } },
            { "Wind", new [,] { { "Top", "1" }, { "Left", "2" }, { "Bottom", "3" }, { "Right", "4" }, {"Top2", "5"} } },
            { "Lightning", new [,] { { "Left", "1" }, { "Right", "2" }, { "BottomLeft", "3" }, { "Top", "4" }, { "BottomRight", "5" }, {"Left2", "6"}} },
            { "Free", new [,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
        };
    }

    private static async void InitDelay()
    {
        await Task.Delay(500);
    }

    public void GameStart()
    {
        audioSource.PlayOneShot(clips[0]);
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
    }

    public async Task OnHit(string hitTargetName)
    {
        var result = await roundManager.Rounds(playerElement, hitTargetName);
        if (result == null) return;
        attack.Type(result);
    }

    public static void IncrementCount()
    {
        RoundManager.Count++;
    }

    private void Hp()
    {
        if (currentHp == beforeHp) return;
        else if (currentHp == 0)
        {
            audioSource.Stop();
            enemy.DestroyEnemy();
            particle.DestroyParticle();
            scene.GameOver();
            return;
        }

        var hp = GameObject.Find("HP" + beforeHp);
        hpList.Add(hp);
        hp.SetActive(false);

        beforeHp = currentHp;
    }

    public void DecreaseHp()
    {
        currentHp--;
        shakeCamera.CameraShaker();

        Hp();
    }

    public void Rounds()
    {
        Target.DestroyTargets();

        switch (++Round)
        {
            case 1:
                playerElement = "Fire";
                break;
            case 2:
                playerElement = "Aqua";
                break;
            case 3:
                playerElement = "Wind";
                break;
            case 4:
                audioSource.Stop();
                audioSource.PlayOneShot(clips[1]);
                playerElement = "Free";
                break;
            case 5:
            case 6:
                playerElement = "Free";
                break;
            case 7:
                playerElement = "Lightning";
                break;
            default:
                scene.GameClear();
                break;
        }

        RoundManager.isReset = false;
        target.GenerateTargets(Magics[playerElement]);
        enemy.Summon(playerElement);
    }

    public void Reset()
    {
        Round = 0;
        currentHp = 3;
        beforeHp = 3;
        timer.TimerReset();
        audioSource.Stop();
        enemy.DestroyEnemy();
        particle.DestroyParticle();

        foreach (var hp in hpList)
        {
            hp.SetActive(true);
        }
    }
}
