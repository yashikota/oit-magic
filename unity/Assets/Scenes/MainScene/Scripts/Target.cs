using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    Camera mainCamera;
    private Dictionary<string, Vector3> targetPositions;


    void Start()
    {
        mainCamera = Camera.main;

        SetPosition();
    }

    private void SetPosition()
    {
        const float center = 0.5f;
        const float offset = 0.15f;
        const float position = 0.02f;
        float nearClipPlane = mainCamera.nearClipPlane + position;

        targetPositions = new Dictionary<string, Vector3>
        {
            { "Top", mainCamera.ViewportToWorldPoint(new Vector3(center, 1 - offset, nearClipPlane))},
            { "Bottom", mainCamera.ViewportToWorldPoint(new Vector3(center, offset, nearClipPlane)) },
            { "Left", mainCamera.ViewportToWorldPoint(new Vector3(offset, center, nearClipPlane)) },
            { "Right", mainCamera.ViewportToWorldPoint(new Vector3(1 - offset, center, nearClipPlane)) },
            { "BottomLeft", mainCamera.ViewportToWorldPoint(new Vector3(center - offset, offset, nearClipPlane)) },
            { "BottomRight", mainCamera.ViewportToWorldPoint(new Vector3(center + offset, offset, nearClipPlane)) }
        };
    }

    private void GenerateTarget(string targetName, string targetNumber)
    {
        if (targetPositions.TryGetValue(targetName, out Vector3 targetPosition))
        {
            // target
            GameObject target = Instantiate(targetPrefab, targetPosition, Quaternion.identity);
            target.name = targetName;
            target.transform.LookAt(mainCamera.transform);

            // text
            TextMeshProUGUI textMeshPro = target.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.name = targetName + "Text";
            textMeshPro.text = targetNumber;
            textMeshPro.transform.LookAt(mainCamera.transform);
        }
    }

    public void GenerateTargets(string[,] targets)
    {
        for (int i = 0; i < targets.GetLength(0); i++)
        {
            GenerateTarget(targets[i, 0], targets[i, 1]);
        }
    }

    public void DestroyTargets()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in targets)
        {
            Destroy(target);
        }
    }

    public void ChangeColor(string targetName, Color color)
    {
        GameObject target = GameObject.Find(targetName);
        if (target != null)
        {
            target.GetComponent<Renderer>().material.color = color;
        }
    }
}
