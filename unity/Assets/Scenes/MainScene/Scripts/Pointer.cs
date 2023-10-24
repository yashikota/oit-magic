using UnityEngine;
using TMPro;

public class Pointer : MonoBehaviour
{
    private Vector3 initializePosition;

    private GameManager gameManager;
    private UDPReceive udpReceiver;
    private CircleTexture circleTexture;
    private Camera mainCamera;
    private GameObject pointer;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        udpReceiver = FindObjectOfType<UDPReceive>();
        circleTexture = FindObjectOfType<CircleTexture>();
        pointer = new("Pointer");

        mainCamera = Camera.main;

        SetPosition();
        GeneratePointer();
    }

    private void SetPosition()
    {
        const float center = 0.5f;
        const float position = 0.05f;
        float nearClipPlane = mainCamera.nearClipPlane + position;

        initializePosition = mainCamera.ViewportToWorldPoint(new Vector3(center, center, nearClipPlane));
    }

    private void GeneratePointer()
    {
        const float radius = 0.015f;

        SpriteRenderer spriteRenderer = pointer.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(circleTexture.GenerateCircleTexture(), new Rect(0, 0, CircleTexture.textureSize, CircleTexture.textureSize), new Vector2(0.5f, 0.5f), CircleTexture.textureSize);
        spriteRenderer.color = new Color(0, 0, 1, 1);
        spriteRenderer.sortingOrder = 2;

        pointer.transform.position = initializePosition;
        pointer.transform.localScale = new Vector3(radius, radius, 1);
        pointer.transform.LookAt(mainCamera.transform);
        pointer.tag = "Pointer";
    }

    private void Update()
    {
        string coordinateData = udpReceiver.GetCoordinate();
        const int attenuationRate = 250;

        if (!string.IsNullOrEmpty(coordinateData))
        {
            string[] coordinateParts = coordinateData.Split(',');
            if (coordinateParts.Length == 2)
            {
                float x = float.Parse(coordinateParts[0]) / attenuationRate;
                float y = float.Parse(coordinateParts[1]) / attenuationRate;

                pointer.transform.position = new Vector3(initializePosition.x + x, initializePosition.y + y, initializePosition.z);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            string objectName = other.gameObject.name;
            gameManager.HitTarget(objectName);
        }
    }
}
