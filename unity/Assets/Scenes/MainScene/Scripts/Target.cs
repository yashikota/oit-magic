using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    GameObject circleObject;
    private Dictionary<int, Vector3> targets;

    void Start()
    {
        circleObject = new GameObject("Circle");

        SetCoordinate();
        CreateCircleSprite();
        GenerateSprites(1, 3, 4);
        GenerateSpritesNumber(new Dictionary<int, int> { { 1, 1 }, { 4, 2 }, { 3, 3 } });
    }

    private void SetCoordinate()
    {
        Camera mainCamera = Camera.main;
        const float center = 0.5f;
        const float offset = 0.15f;
        const float position = 0.01f;
        float nearClipPlane = mainCamera.nearClipPlane + position;

        // Generate target positions
        Vector3 top = mainCamera.ViewportToWorldPoint(new Vector3(center, 1 - offset, nearClipPlane));
        Vector3 bottom = mainCamera.ViewportToWorldPoint(new Vector3(center, offset, nearClipPlane));
        Vector3 left = mainCamera.ViewportToWorldPoint(new Vector3(offset, center, nearClipPlane));
        Vector3 right = mainCamera.ViewportToWorldPoint(new Vector3(1 - offset, center, nearClipPlane));
        Vector3 leftBottom = mainCamera.ViewportToWorldPoint(new Vector3(center - offset, offset, nearClipPlane));
        Vector3 rightBottom = mainCamera.ViewportToWorldPoint(new Vector3(center + offset, offset, nearClipPlane));

        targets = new Dictionary<int, Vector3>
        {
            { 1, top },
            { 2, left },
            { 3, right },
            { 4, bottom },
            { 5, leftBottom },
            { 6, rightBottom }
        };
    }

    private void CreateCircleSprite()
    {
        const float radius = 0.01f;
        const int textureSize = 256;

        SpriteRenderer spriteRenderer = circleObject.AddComponent<SpriteRenderer>();

        Texture2D circleTexture = new(textureSize, textureSize)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };

        for (int x = 0; x < circleTexture.width; x++)
        {
            for (int y = 0; y < circleTexture.height; y++)
            {
                if (Vector2.Distance(new Vector2(x, y), new Vector2(circleTexture.width / 2, circleTexture.height / 2)) <= circleTexture.width / 2)
                {
                    circleTexture.SetPixel(x, y, Color.white);
                }
                else
                {
                    circleTexture.SetPixel(x, y, Color.clear);
                }
            }
        }
        circleTexture.Apply();

        spriteRenderer.sprite = Sprite.Create(circleTexture, new Rect(0, 0, circleTexture.width, circleTexture.height), Vector2.one * 0.5f);

        circleObject.transform.localScale = new Vector3(radius, radius, 1.0f);
    }

    public void GenerateSprites(params int[] spriteNumbers)
    {
        int spriteLayer = 0;
        foreach (int spriteNumber in spriteNumbers)
        {
            if (targets.ContainsKey(spriteNumber))
            {
                Vector3 targetPosition = targets[spriteNumber];
                GameObject newCircle = Instantiate(circleObject, targetPosition, Quaternion.identity);
                newCircle.name = "Target" + spriteNumber;

                // Set layer
                SpriteRenderer spriteRenderer = newCircle.GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = spriteLayer;
                spriteLayer++;
            }
        }
    }

    public void GenerateSpritesNumber(Dictionary<int, int> spriteNumbers)
    {
        int textLayer = 1;
        foreach (KeyValuePair<int, int> spriteNumber in spriteNumbers)
        {
            if (targets.ContainsKey(spriteNumber.Key))
            {
                Vector3 targetPosition = targets[spriteNumber.Key];
                GameObject textObject = new("Text")
                {
                    name = "Text" + spriteNumber.Key
                };

                TextMeshPro textMesh = textObject.AddComponent<TextMeshPro>();
                textMesh.text = spriteNumber.Value.ToString();
                textMesh.fontSize = 2.5f;
                textObject.transform.position = targetPosition;
                textObject.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                textMesh.color = new Color(0, 0, 0, 1);
                textMesh.alignment = TextAlignmentOptions.Center;
                textMesh.alignment = TextAlignmentOptions.Midline;

                // Set layer
                textMesh.sortingOrder = textLayer;
                textLayer++;
            }
        }
    }
}
