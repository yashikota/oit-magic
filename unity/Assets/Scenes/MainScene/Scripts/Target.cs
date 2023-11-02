using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Target : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> handle;

    private Camera mainCamera;
    private Dictionary<string, Vector3> targetPositions;

    private void Start()
    {
        mainCamera = Camera.main;

        LoadAsset();
        SetPosition();
    }

    private async void LoadAsset()
    {
        handle = Addressables.LoadAssetAsync<GameObject>("TargetPrefab");
        await handle.Task;
    }

    private void SetPosition()
    {
        const float center = 0.5f;
        const float offset = 0.15f;
        const float position = 0.02f;
        var nearClipPlane = mainCamera.nearClipPlane + position;

        targetPositions = new Dictionary<string, Vector3>
        {
            { "Top", mainCamera.ViewportToWorldPoint(new Vector3(center, 1 - offset, nearClipPlane))},
            { "Bottom", mainCamera.ViewportToWorldPoint(new Vector3(center, offset, nearClipPlane)) },
            // { "Left", mainCamera.ViewportToWorldPoint(new Vector3(offset, center, nearClipPlane)) },
            // { "Right", mainCamera.ViewportToWorldPoint(new Vector3(1 - offset, center, nearClipPlane)) },
            { "Left", mainCamera.ViewportToWorldPoint(new Vector3(center - 0.25f, center, nearClipPlane)) },
            { "Right", mainCamera.ViewportToWorldPoint(new Vector3(center + 0.25f, center, nearClipPlane)) },
            { "BottomLeft", mainCamera.ViewportToWorldPoint(new Vector3(center - offset, offset, nearClipPlane)) },
            { "BottomRight", mainCamera.ViewportToWorldPoint(new Vector3(center + offset, offset, nearClipPlane)) }
        };
    }

    private void GenerateTarget(string targetName, string targetNumber)
    {
        if (!targetPositions.TryGetValue(targetName, out var targetPosition) || targetName.EndsWith("2")) return;

        GameObject targetPrefab = null;
        if (handle.Status == AsyncOperationStatus.Succeeded) targetPrefab = handle.Result;
        if (targetPrefab == null) return;

        // target
        var target = Instantiate(targetPrefab, targetPosition, Quaternion.identity);
        target.name = targetName;
        target.transform.LookAt(mainCamera.transform);

        // canvas
        var canvas = target.transform.GetChild(0).gameObject;
        canvas.GetComponent<Canvas>().worldCamera = mainCamera;

        // text
        var textMeshPro = target.GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.name = targetName + "Text";
        textMeshPro.text = targetNumber;
        textMeshPro.transform.position = targetPositions[targetName];
        textMeshPro.transform.rotation = Quaternion.LookRotation(textMeshPro.transform.position - mainCamera.transform.position);
    }

    public void GenerateTargets(string[,] targets)
    {
        for (var i = 0; i < targets.GetLength(0); i++)
        {
            GenerateTarget(targets[i, 0], targets[i, 1]);
        }
    }

    public static void DestroyTargets()
    {
        var targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (var target in targets)
        {
            Destroy(target);
        }
    }

    public static void ChangeColor(string targetName, Color color)
    {
        var target = GameObject.Find(targetName);
        if (target != null)
        {
            target.GetComponent<Renderer>().material.color = color;
        }
    }

    public static void ChangeText(string targetName, string text)
    {
        var target = GameObject.Find(targetName);
        if (target == null) return;

        var textMeshPro = target.GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = text;
    }

    private void OnApplicationQuit()
    {
        if (handle.IsValid())
        {
            Addressables.Release(handle);
        }
    }
}
