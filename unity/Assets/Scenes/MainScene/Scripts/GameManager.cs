using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Target target;
    private Pointer pointer;

    private int round = 1;
    Dictionary<string, Vector3> targetPositions;
    Vector3 pointerPosition;

    void Start()
    {
        target = FindObjectOfType<Target>();
        pointer = FindObjectOfType<Pointer>();

        targetPositions = target.GetTargetPositions();

        GameCycle();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameCycle();
        }

        pointerPosition = pointer.GetPointerPosition();
        CheckTargetCollision();
    }

    private string CheckTargetCollision()
    {
        Dictionary<string, float> targetThresholds = new Dictionary<string, float> {
            {"Top", 0.01f},
            {"Bottom", 0.01f},
            {"Left", 0.01f},
            {"Right", 0.01f},
            {"BottomLeft", 0.01f},
            {"BottomRight", 0.01f}
        };

        foreach (var kvp in targetPositions)
        {
            string targetName = kvp.Key;
            Vector3 targetPosition = kvp.Value;
            float distance = Vector3.Distance(pointerPosition, targetPosition);

            if (distance <= targetThresholds[targetName])
            {
                Debug.Log("Hit " + targetName);
                return targetName;
            }
        }
        return null;
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
