using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    Camera mainCamera;
    private Dictionary<string, Vector3> targetCoordinates;
    private const int textureSize = 256;

    void Start()
    {
        mainCamera = Camera.main;
        SetCoordinate();
    }

    private void SetCoordinate()
    {
        const float center = 0.5f;
        const float offset = 0.15f;
        const float position = 0.01f;
        float nearClipPlane = mainCamera.nearClipPlane + position;

        targetCoordinates = new Dictionary<string, Vector3>
        {
            { "Top", mainCamera.ViewportToWorldPoint(new Vector3(center, 1 - offset, nearClipPlane))},
            { "Bottom", mainCamera.ViewportToWorldPoint(new Vector3(center, offset, nearClipPlane)) },
            { "Left", mainCamera.ViewportToWorldPoint(new Vector3(offset, center, nearClipPlane)) },
            { "Right", mainCamera.ViewportToWorldPoint(new Vector3(1 - offset, center, nearClipPlane)) },
            { "BottomLeft", mainCamera.ViewportToWorldPoint(new Vector3(center - offset, offset, nearClipPlane)) },
            { "BottomRight", mainCamera.ViewportToWorldPoint(new Vector3(center + offset, offset, nearClipPlane)) }
        };
    }

    private Texture2D GenerateCircleTexture()
    {
        Texture2D circleTexture = new(textureSize, textureSize)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };

        Vector2 circleCenter = new(textureSize / 2, textureSize / 2);

        for (int x = 0; x < circleTexture.width; x++)
        {
            for (int y = 0; y < circleTexture.height; y++)
            {
                Vector2 pixelPosition = new(x, y);
                float distance = Vector2.Distance(pixelPosition, circleCenter);
                circleTexture.SetPixel(x, y, distance <= textureSize / 2 ? Color.white : Color.clear);
            }
        }

        circleTexture.Apply();

        return circleTexture;
    }

    private void GenerateTarget(string target, string targetNumber)
    {
        if (targetCoordinates.TryGetValue(target, out Vector3 targetPosition))
        {
            // Sprite
            const float radius = 0.025f;
            GameObject spriteObject = new("Sprite")
            {
                name = "TargetSprite" + targetNumber
            };
            SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprite.Create(GenerateCircleTexture(), new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f), textureSize);
            spriteObject.transform.position = targetPosition;
            spriteObject.transform.localScale = new Vector3(radius, radius, 1);
            spriteObject.transform.LookAt(mainCamera.transform);
            spriteObject.tag = "Target";
            spriteRenderer.color = new Color(1, 1, 1, 1);
            spriteRenderer.sortingOrder = 0;

            // Text
            GameObject textObject = new("Text")
            {
                name = "TargetText" + targetNumber
            };
            TextMeshPro textMesh = textObject.AddComponent<TextMeshPro>();
            textMesh.text = targetNumber.ToString();
            textMesh.fontSize = 2.5f;
            textObject.transform.position = targetPosition;
            textObject.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            textObject.transform.rotation = Quaternion.LookRotation(textObject.transform.position - mainCamera.transform.position);
            textObject.tag = "Target";
            textMesh.color = new Color(0, 0, 0, 1);
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.alignment = TextAlignmentOptions.Midline;
            textMesh.sortingOrder = 1;
        }
    }

    public void GenerateTargets(string[,] targets)
    {
        for (int i = 0; i < targets.GetLength(0); i++)
        {
            GenerateTarget(targets[i, 0], targets[i, 1]);
        }
    }

    public void RewriteTargetNumber(string targetNumber, string newTargetNumber)
    {
        GameObject targetText = GameObject.Find("TargetText" + targetNumber);
        targetText.name = "TargetText" + newTargetNumber;
        targetText.GetComponent<TextMeshPro>().text = newTargetNumber;
    }

    public void DestroyTargets()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in targets)
        {
            Destroy(target);
        }
    }
}
