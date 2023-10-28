using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] GameObject pointerSprite;
    private Vector3 initializePosition;

    private GameManager gameManager;
    private UDPManager udpManager;
    private Camera mainCamera;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        udpManager = FindObjectOfType<UDPManager>();
        mainCamera = Camera.main;

        SetPosition();
    }

    private void SetPosition()
    {
        const float center = 0.5f;
        const float position = 0.02f;
        float nearClipPlane = mainCamera.nearClipPlane + position;

        initializePosition = mainCamera.ViewportToWorldPoint(new Vector3(center, center, nearClipPlane));
    }

    private void Update()
    {
        string coordinateData = udpManager.GetCoordinate();
        const int attenuationRate = 250;

        if (!string.IsNullOrEmpty(coordinateData))
        {
            string[] coordinateParts = coordinateData.Split(',');
            if (coordinateParts.Length == 2)
            {
                float x = float.Parse(coordinateParts[0]) / attenuationRate;
                float y = float.Parse(coordinateParts[1]) / attenuationRate;

                Vector3 position = new Vector3(x, y, 0) + initializePosition;
                pointerSprite.transform.position = position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            string targetName = other.gameObject.name;
            gameManager.OnHit(targetName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            gameManager.IncrementCount();
        }
    }
}
