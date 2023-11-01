using UnityEngine;
using DG.Tweening;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;

    private readonly float shakeDuration = 0.3f;

    public void CameraShaker()
    {
        cam.DOComplete();
        cam.DOShakePosition(shakeDuration, positionStrength);
        cam.DOShakeRotation(shakeDuration, rotationStrength);
    }
}
