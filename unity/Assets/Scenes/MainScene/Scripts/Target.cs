using UnityEngine;

public class SpriteGenerator : MonoBehaviour
{
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private float spriteScale = 0.025f;

    void Start()
    {
        Camera mainCamera = Camera.main;

        const float center = 0.5f;
        const float offset = 0.15f;

        Vector3 top = mainCamera.ViewportToWorldPoint(new Vector3(center, 1 - offset, mainCamera.nearClipPlane));
        Vector3 bottom = mainCamera.ViewportToWorldPoint(new Vector3(center, offset, mainCamera.nearClipPlane));
        Vector3 left = mainCamera.ViewportToWorldPoint(new Vector3(offset, center, mainCamera.nearClipPlane));
        Vector3 right = mainCamera.ViewportToWorldPoint(new Vector3(1 - offset, center, mainCamera.nearClipPlane));

        GenerateSprite(top, 1);
        GenerateSprite(left, 2);
        GenerateSprite(right, 3);
        GenerateSprite(bottom, 4);
    }

    void GenerateSprite(Vector3 position, int spriteNumber)
    {
        GameObject sprite = Instantiate(spriteObject, position, Quaternion.identity);
        sprite.transform.localScale = new Vector3(spriteScale, spriteScale, 1.0f);
        sprite.name = "Sprite" + spriteNumber;
    }
}
