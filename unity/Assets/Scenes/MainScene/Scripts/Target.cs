using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Target : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> handle;

    Camera mainCamera;
    private Dictionary<string, Vector3> targetPositions;

    void Start()
    {
        mainCamera = Camera.main;

        LoadAsset();
        SetPosition();
    }

    async void LoadAsset()
    {
        handle = Addressables.LoadAssetAsync<GameObject>("TargetPrefab");
        await handle.Task;
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
            GameObject targetPrefab = null;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                targetPrefab = handle.Result;
            }

            // target
            GameObject target = Instantiate(targetPrefab, targetPosition, Quaternion.identity);
            target.name = targetName;
            target.transform.LookAt(mainCamera.transform);

            // canvas
            GameObject canvas = target.transform.GetChild(0).gameObject;
            canvas.GetComponent<Canvas>().worldCamera = mainCamera;

            // text
            TextMeshProUGUI textMeshPro = target.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.name = targetName + "Text";
            textMeshPro.text = targetNumber;
            textMeshPro.transform.position = targetPositions[targetName];
            textMeshPro.transform.rotation = Quaternion.LookRotation(textMeshPro.transform.position - mainCamera.transform.position);
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

    private void OnDestory()
    {
        if (handle.IsValid())
        {
            Addressables.Release(handle);
        }
    }
}
