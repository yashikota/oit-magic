using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private string element;
    private string hitTargetName;
    private int round;
    public static int Count = 1;
    private List<string> checkedList;
    private Dictionary<string, bool> elements;

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

    public string Round(string element, string hitTargetName, int round)
    {
        this.element = element;
        this.hitTargetName = hitTargetName;
        this.round = round;

        // target is not selected
        if (!checkedList.Any()) Count = 1;

        firstTargetName = GameManager.Magics[element][0, 0];
        targetLength = GameManager.Magics[element].GetLength(0);
        lastTargetNumber = GameManager.Magics[element][targetLength - 1, 1];

        // already selected
        if (checkedList.Contains(hitTargetName))
        {
            Count--;
            return null;
        }

        if (round is 1 or 2 or 3 or 7) return NormalRound();
        else return FreeRound();
    }

    private string NormalRound()
    {
        currentTargetName = GameManager.Magics[element][Count - 1, 0].Replace("2", "");

        // start and end are same
        if (targetLength == Count + 1 && round is 3 or 7)
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
            Reset();

            return element;
        }
        else
        {
            Target.ChangeColor(hitTargetName, Color.red);
            Reset();

            foreach (var key in GameManager.Magics[element].Cast<string>().Where((v, i) => i % 2 == 0).Select(v => v.Replace("2", "")).Except(checkedList))
            {
                Target.ChangeColor(key, Color.white);
            }

            return null;
        }
    }

    private string FreeRound()
    {
        int limit = 4;

        // start and end are same
        if (elements["Wind"] && Count == 4)
        {
            checkedList.Remove(firstTargetName);
            Target.ChangeColor(firstTargetName, Color.white);
            Target.ChangeText(firstTargetName, lastTargetNumber);
        }

        // Fire, Aqua, Wind
        foreach (var key in new List<string>(elements.Keys))
        {
            if (Count >= limit) break;
            if (GameManager.Magics[key][Count - 1, 0] != hitTargetName) elements[key] = false;
        }

        if (elements.Any(v => v.Value))
        {
            if (elements["Wind"]) limit = 5;
            Target.ChangeColor(hitTargetName, Color.blue);
            checkedList.Add(hitTargetName);

            if (Count < limit) return null;
            foreach (var key in new List<string>(elements.Keys))
            {
                if (elements[key])
                {
                    Reset();
                    return key;
                }
            }
        }
        else
        {
            Target.ChangeColor(hitTargetName, Color.red);
            Reset();
            foreach (var key in GameManager.Magics[element].Cast<string>().Where((v, i) => i % 2 == 0).Select(v => v.Replace("2", "")).Except(checkedList))
            {
                Target.ChangeColor(key, Color.white);
            }
        }

        return null;
    }

    private void Reset()
    {
        Count = 1;
        checkedList.Clear();

        elements["Fire"] = true;
        elements["Aqua"] = true;
        elements["Wind"] = true;
    }
}