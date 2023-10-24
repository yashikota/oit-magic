using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject spriteObject;
    private Vector3 initialPosition;

    private UDPReceive udpReceiver;
    private Camera mainCamera;

    private void Start()
    {
        initialPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        udpReceiver = FindObjectOfType<UDPReceive>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        string coordinateData = udpReceiver.GetCoordinate();
        const int attenuationRate = 300;

        if (!string.IsNullOrEmpty(coordinateData))
        {
            string[] coordinateParts = coordinateData.Split(',');
            if (coordinateParts.Length == 2)
            {
                float x = float.Parse(coordinateParts[0]) / attenuationRate;
                float y = float.Parse(coordinateParts[1]) / attenuationRate;

                Vector3 newPosition = initialPosition + new Vector3(x, y, 0);
                float cameraZ = mainCamera.transform.position.z;
                newPosition.z = cameraZ + 0.5f;
                spriteObject.transform.position = newPosition;
            }
        }
    }
}
