using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Target target;
    private int round = 1;

    void Start()
    {
        target = FindObjectOfType<Target>();
        GameCycle();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameCycle();
        }
    }

    private void GameCycle()
    {
        // positions
        string[,] round1 = new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Right", "3" } };
        string[,] round2 = new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Left", "3" }, { "Right", "4" } };
        string[,] round3 = new string[,] { { "Top", "1" }, { "Left", "2" }, { "Bottom", "3" }, { "Right", "4" } };
        string[,] round456 = new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } };
        string[,] round7 = new string[,] { { "Left", "1" }, { "Right", "2" }, { "BottomLeft", "3" }, { "Top", "4" }, { "BottomRight", "5" } };

        // destroy targets
        target.DestroyTargets();

        // generate targets
        switch (round)
        {
            case 1:
                target.GenerateTargets(round1);
                break;
            case 2:
                target.GenerateTargets(round2);
                break;
            case 3:
                target.GenerateTargets(round3);
                // target.RewriteTargetNumber("1", "5");
                break;
            case 4:
            case 5:
            case 6:
                target.GenerateTargets(round456);
                break;
            case 7:
                target.GenerateTargets(round7);
                // target.RewriteTargetNumber("1", "6");
                break;
            default:
                break;
        }
        round++;
    }
}
