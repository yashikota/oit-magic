using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject spriteObject;
    private Vector3 initialPosition;

    private UDPReceive udpReceiver;

    private void Start()
    {
        initialPosition = spriteObject.transform.position;
        udpReceiver = FindObjectOfType<UDPReceive>();
    }

    private void Update()
    {
        string coordinateData = udpReceiver.GetCoordinate();
        const int attenuationRate = 100;

        if (!string.IsNullOrEmpty(coordinateData))
        {
            string[] coordinateParts = coordinateData.Split(',');
            if (coordinateParts.Length == 2)
            {
                float x = float.Parse(coordinateParts[0]) / attenuationRate;
                float y = float.Parse(coordinateParts[1]) / attenuationRate;

                Vector3 newPosition = initialPosition + new Vector3(x, y, 0);
                spriteObject.transform.position = newPosition;
            }
        }
    }
}
