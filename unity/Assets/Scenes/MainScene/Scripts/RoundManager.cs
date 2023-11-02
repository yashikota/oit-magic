using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private string PlayerElement;
    private string hitTargetName;
    public static int Count = 1;
    private List<string> checkedList;
    private Dictionary<string, bool> elements;
    public static bool isReset;

    private string firstTargetName;
    private string currentTargetName;
    private int targetLength;
    private string lastTargetNumber;

    private void Start()
    {
        checkedList = new List<string>();

        elements = new Dictionary<string, bool> {
            { "Fire", true },
            { "Aqua", true },
            { "Wind", true },
        };
    }

    public async Task<string> Rounds(string elem, string target)
    {
        PlayerElement = elem;
        hitTargetName = target;

        if (isReset) return null;

        // target is not selected
        if (!checkedList.Any()) Count = 1;

        firstTargetName = GameManager.Magics[PlayerElement][0, 0];
        targetLength = GameManager.Magics[PlayerElement].GetLength(0);
        lastTargetNumber = GameManager.Magics[PlayerElement][targetLength - 1, 1];

        // already selected
        if (checkedList.Contains(hitTargetName)) { Count--; return null; }

        return IsNormalRound() ? await NormalRound() : await FreeRound();
    }

    private static bool IsNormalRound()
    {
        return GameManager.Round is 1 or 2 or 3 or 7;
    }

    private async Task<string> NormalRound()
    {
        currentTargetName = GameManager.Magics[PlayerElement][Count - 1, 0].Replace("2", "");

        // start and end are same
        if (targetLength == Count + 1 && GameManager.Round is 3 or 7)
        {
            checkedList.Remove(firstTargetName);
            Target.ChangeColor(firstTargetName, Color.white);
            Target.ChangeText(firstTargetName, lastTargetNumber);
        }

        if (hitTargetName == currentTargetName)
        {
            Target.ChangeColor(hitTargetName, Color.blue);
            checkedList.Add(hitTargetName);
            if (Count != targetLength) return null;
            isReset = true;
            Reset();

            return PlayerElement;
        }
        else
        {
            Target.ChangeColor(hitTargetName, Color.red);
            await Task.Delay(500);
            Reset();

            foreach (var key in GameManager.Magics[PlayerElement].Cast<string>()
                .Where((_, i) => i % 2 == 0)
                .Select(v => v.Replace("2", ""))
                .Except(checkedList))
            {
                Target.ChangeColor(key, Color.white);
            }

            return null;
        }
    }

    private async Task<string> FreeRound()
    {
        var limit = 4;

        // start and end are same
        if (elements["Wind"] && Count == 4)
        {
            checkedList.Remove(firstTargetName);
            Target.ChangeColor(firstTargetName, Color.white);
            Target.ChangeText(firstTargetName, lastTargetNumber);
        }

        // Fire, Aqua, Wind
        foreach (var key in new List<string>(elements.Keys)
            .TakeWhile(_ => Count < limit)
            .Where(key => GameManager.Magics[key][Count - 1, 0] != hitTargetName))
        {
            elements[key] = false;
        }

        if (elements.Any(v => v.Value))
        {
            if (elements["Wind"]) limit = 5;
            Target.ChangeColor(hitTargetName, Color.blue);
            checkedList.Add(hitTargetName);

            if (Count < limit) return null;
            foreach (var key in new List<string>(elements.Keys).Where(key => elements[key]))
            {
                Reset();
                return key;
            }
        }
        else
        {
            Target.ChangeColor(hitTargetName, Color.red);
            await Task.Delay(500);
            Reset();
            foreach (var key in GameManager.Magics[PlayerElement].Cast<string>()
                .Where((_, i) => i % 2 == 0).Select(v => v.Replace("2", ""))
                .Except(checkedList))
            {
                Target.ChangeColor(key, Color.white);
            }
        }

        return null;
    }

    public async void Reset()
    {
        Count = 1;
        checkedList.Clear();

        elements["Fire"] = true;
        elements["Aqua"] = true;
        elements["Wind"] = true;

        enemy.SetElements();

        await Task.Delay(1000);
    }
}
