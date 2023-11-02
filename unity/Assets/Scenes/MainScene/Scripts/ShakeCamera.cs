using UnityEngine;
using DG.Tweening;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;

    private const float ShakeDuration = 0.3f;

    public void CameraShaker()
    {
        cam.DOComplete();
        cam.DOShakePosition(ShakeDuration, positionStrength);
        cam.DOShakeRotation(ShakeDuration, rotationStrength);
    }
}
