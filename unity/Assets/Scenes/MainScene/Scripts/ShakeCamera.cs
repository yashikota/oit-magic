using UnityEngine;
using DG.Tweening;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;

    private readonly float shakeDuration = 0.3f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            CameraShaker();
    }

    private void CameraShaker()
    {
        cam.DOComplete();
        cam.DOShakePosition(shakeDuration, positionStrength);
        cam.DOShakeRotation(shakeDuration, rotationStrength);
    }
}
