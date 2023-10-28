using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Target target;
    [SerializeField] private UDPManager udpManager;
    [SerializeField] private Magic magic;

    private int round = 0;
    private int count = 1;
    string element;
    bool[] results;
    List<string> checkedList;

    void Start()
    {
        checkedList = new List<string>();

        Init();
    }

    private async void Init()
    {
        await Task.Delay(500);
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
        if (round == 4 || round == 5 || round == 6)
        {
            results = magic.TypeDetect(hitTargetName, count);
            if (results.Contains(true))
            {
                target.ChangeColor(hitTargetName, Color.blue);
                count++;
            }
            else
            {
                target.ChangeColor(hitTargetName, Color.red);
                count = 1;
            }
        }

        // target is not selected
        if (!checkedList.Any())
        {
            count = 1;
        }

        string firstTargetName = Magic.magics[element][0, 0];
        string currentTargetName = Magic.magics[element][count - 1, 0].Replace("2", "");
        int targetLength = Magic.magics[element].GetLength(0);
        string lastTargetNumber = Magic.magics[element][targetLength - 1, 1];

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
            foreach (string key in Magic.magics[element].Cast<string>().Where((v, i) => i % 2 == 0).Select(v => v.Replace("2", "")))
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
                element = "Fire";
                target.GenerateTargets(Magic.magics[element]);
                break;
            case 2:
                element = "Aqua";
                target.GenerateTargets(Magic.magics[element]);
                break;
            case 3:
                element = "Wind";
                target.GenerateTargets(Magic.magics[element]);
                break;
            case 4:
            case 5:
            case 6:
                element = "Free";
                target.GenerateTargets(Magic.magics[element]);
                break;
            case 7:
                element = "Lightning";
                target.GenerateTargets(Magic.magics[element]);
                break;
            default:
                break;
        }
    }
}
