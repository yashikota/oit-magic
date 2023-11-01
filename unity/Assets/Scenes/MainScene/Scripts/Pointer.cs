using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject pointerSprite;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UDPManager udpManager;
    [SerializeField] private Scene scene;

    private Camera mainCamera;
    private Vector3 initializePosition;
    private float touchTime = 0f;

    private void Start()
    {
        mainCamera = Camera.main;

        SetPosition();
    }

    private void SetPosition()
    {
        const float center = 0.5f;
        const float position = 0.02f;
        var nearClipPlane = mainCamera.nearClipPlane + position;

        initializePosition = mainCamera.ViewportToWorldPoint(new Vector3(center, center, nearClipPlane));
    }

    public void ResetPosition()
    {
        pointerSprite.transform.position = initializePosition;
    }

    private void Update()
    {
        var coordinateData = udpManager.GetCoordinate();
        const int attenuationRate = 250;
        const float rangeFromCamera = 0.1f;

        if (string.IsNullOrEmpty(coordinateData)) return;

        var coordinateParts = coordinateData.Split(',');
        if (coordinateParts.Length != 2) return;

        var x = float.Parse(coordinateParts[0]) / attenuationRate;
        var y = float.Parse(coordinateParts[1]) / attenuationRate;

        var position = new Vector3(x, y, rangeFromCamera) + initializePosition;
        pointerSprite.transform.position = position;
        pointerSprite.transform.LookAt(mainCamera.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StartButton") || other.gameObject.CompareTag("EndButton")) touchTime = Time.time;
        if (!other.gameObject.CompareTag("Target")) return;

        var targetName = other.gameObject.name;
        gameManager.OnHit(targetName);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StartButton") || other.gameObject.CompareTag("EndButton")) touchTime = 0f;
        if (!other.gameObject.CompareTag("Target")) return;

        gameManager.IncrementCount();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("StartButton"))
        {
            // more than 2 seconds to start
            if (Time.time - touchTime > 2f) scene.OnClickStart();
            return;
        }
        else if (other.gameObject.CompareTag("EndButton"))
        {
            // more than 2 seconds to end
            if (Time.time - touchTime > 2f) scene.OnClickEnd();
            return;
        }
    }
}
