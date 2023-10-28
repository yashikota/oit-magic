using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public static Dictionary<string, string[,]> magics;
    private string[] elements;

    void Start()
    {
        magics = new Dictionary<string, string[,]>
        {
            { "Free", new string[,] { { "Top", "" }, { "Bottom", "" }, { "Left", "" }, { "Right", "" } } },
            { "Fire", new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Right", "3" } } },
            { "Aqua", new string[,] { { "Top", "1" }, { "Bottom", "2" }, { "Left", "3" }, { "Right", "4" } } },
            { "Wind", new string[,] { { "Top", "1" }, { "Left", "2" }, { "Bottom", "3" }, { "Right", "4" }, {"Top2", "5"} } },
            { "Lightning", new string[,] { { "Left", "1" }, { "Right", "2" }, { "BottomLeft", "3" }, { "Top", "4" }, { "BottomRight", "5" }, {"Left2", "6"}} },
        };

        elements = new string[] { "Fire", "Aqua", "Wind" };
    }

    public bool[] TypeDetect(string hitTargetName, int count)
    {
        int i = 0;
        bool[] results = new bool[elements.Length];
        foreach (string element in elements)
        {
            if (magics[element][count, 0] == hitTargetName)
            {
                results[i] = true;
            }
            else
            {
                results[i] = false;
            }
            i++;
        }

        return results;
    }
}
