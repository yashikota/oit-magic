using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private float spriteScale = 0.025f;
    private Dictionary<int, Vector3> targets;

    void Start()
    {
        Camera mainCamera = Camera.main;

        const float center = 0.5f;
        const float offset = 0.15f;

        // Generate target positions
        Vector3 top = mainCamera.ViewportToWorldPoint(new Vector3(center, 1 - offset, mainCamera.nearClipPlane));
        Vector3 bottom = mainCamera.ViewportToWorldPoint(new Vector3(center, offset, mainCamera.nearClipPlane));
        Vector3 left = mainCamera.ViewportToWorldPoint(new Vector3(offset, center, mainCamera.nearClipPlane));
        Vector3 right = mainCamera.ViewportToWorldPoint(new Vector3(1 - offset, center, mainCamera.nearClipPlane));
        Vector3 leftBottom = mainCamera.ViewportToWorldPoint(new Vector3(center - offset, offset, mainCamera.nearClipPlane));
        Vector3 rightBottom = mainCamera.ViewportToWorldPoint(new Vector3(center + offset, offset, mainCamera.nearClipPlane));

        targets = new Dictionary<int, Vector3>
        {
            { 1, top },
            { 2, left },
            { 3, right },
            { 4, bottom },
            { 5, leftBottom },
            { 6, rightBottom }
        };

        GenerateSprites(1, 3, 4);
    }

    public void GenerateSprites(params int[] spriteNumbers)
    {
        foreach (int spriteNumber in spriteNumbers)
        {
            if (targets.ContainsKey(spriteNumber))
            {
                Vector3 position = targets[spriteNumber];
                GameObject sprite = Instantiate(spriteObject, position, Quaternion.identity);
                sprite.transform.localScale = new Vector3(spriteScale, spriteScale, 1.0f);
                sprite.name = "Sprite" + spriteNumber;
            }
        }
    }
}
