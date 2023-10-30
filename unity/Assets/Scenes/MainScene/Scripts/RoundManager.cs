using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private string element;
    private string hitTargetName;
    private int round;
    private int count = 1;
    private List<string> checkedList;

    string firstTargetName;
    string currentTargetName;
    int targetLength;
    string lastTargetNumber;

    public async Task<string> Round(string element, string hitTargetName, int round, int count, List<string> checkedList)
    {
        this.element = element;
        this.hitTargetName = hitTargetName;
        this.round = round;
        this.count = count;
        this.checkedList = checkedList;

        // target is not selected
        if (!checkedList.Any())
        {
            count = 1;
        }

        firstTargetName = GameManager.Magics[element][0, 0];
        currentTargetName = GameManager.Magics[element][count - 1, 0].Replace("2", "");
        targetLength = GameManager.Magics[element].GetLength(0);
        lastTargetNumber = GameManager.Magics[element][targetLength - 1, 1];

        if (round is 1 or 2 or 3 or 7)
        {
            return await NormalRound();
        }
        else
        {
            return FreeRound();
        }
    }

    // Round 1, 2, 3, 7
    private async Task<string> NormalRound()
    {
        // start and end are same
        if (targetLength == count + 1 && (round == 3 || round == 7))
        {
            checkedList.Remove(firstTargetName);
            Target.ChangeColor(firstTargetName, Color.white);
            Target.ChangeText(firstTargetName, lastTargetNumber);
        }

        // already selected
        if (checkedList.Contains(hitTargetName))
        {
            count--;
            return "None";
        }

        if (hitTargetName == currentTargetName)
        {
            Target.ChangeColor(hitTargetName, Color.blue);
            checkedList.Add(hitTargetName);

            return "True";
        }
        else
        {
            Target.ChangeColor(hitTargetName, Color.red);
            count = 1;
            checkedList.Clear();

            await Task.Delay(500);
            foreach (string key in GameManager.Magics[element].Cast<string>().Where((v, i) => i % 2 == 0).Select(v => v.Replace("2", "")))
            {
                Target.ChangeColor(key, Color.white);
            }

            return "False";
        }
    }

    private string FreeRound()
    {
        // Fire, Aqua, Windの判定
        foreach (var key in new List<string>(GameManager.Elements.Keys))
        {
            if (GameManager.Magics[key][count - 1, 0] == hitTargetName)
            {
                GameManager.Elements[key] &= true;
            }
            else
            {
                GameManager.Elements[key] = false;
            }
        }

        // foreach (string key in elements.Keys)
        // {
        //     Debug.Log(key + ": " + elements[key]);
        // }

        // 判定
        if (count != 4) return (GameManager.Elements["Fire"] || GameManager.Elements["Aqua"] || GameManager.Elements["Wind"]) ? "True" : "False";

        if (GameManager.Elements["Fire"]) { return "Fire"; }
        else if (GameManager.Elements["Aqua"]) { return "Aqua"; }
        else if (GameManager.Elements["Wind"]) { return "Wind"; }
        else { return "False"; }
    }
}
