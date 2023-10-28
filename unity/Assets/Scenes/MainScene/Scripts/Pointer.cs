using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject pointerSprite;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UDPManager udpManager;

    private Camera mainCamera;
    private Vector3 initializePosition;

    private void Start()
    {
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
        const float rangeFromCamera = 0.1f;

        if (!string.IsNullOrEmpty(coordinateData))
        {
            string[] coordinateParts = coordinateData.Split(',');
            if (coordinateParts.Length == 2)
            {
                float x = float.Parse(coordinateParts[0]) / attenuationRate;
                float y = float.Parse(coordinateParts[1]) / attenuationRate;

                Vector3 position = new Vector3(x, y, rangeFromCamera) + initializePosition;
                pointerSprite.transform.position = position;
                pointerSprite.transform.LookAt(mainCamera.transform);
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
