using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject pointerObject;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UDPManager udpManager;
    [SerializeField] private Scene scene;

    private Camera mainCamera;
    private Vector3 initializePosition;
    private float touchTime;
    private Tweener tweener;

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
        pointerObject.transform.position = initializePosition;
    }

    private void FillAmount()
    {
        tweener = pointerObject.GetComponent<Image>().DOFillAmount(0f, 2f);
    }

    private void ResetFillAmount()
    {
        if (tweener == null) return;
        tweener.Kill();
        pointerObject.GetComponent<Image>().fillAmount = 1f;
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
        pointerObject.transform.position = position;
        pointerObject.transform.LookAt(mainCamera.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StartButton") || other.gameObject.CompareTag("EndButton"))
        {
            touchTime = Time.time;
            FillAmount();
        }
        if (!other.gameObject.CompareTag("Target")) return;

        var targetName = other.gameObject.name;
        gameManager.OnHit(targetName);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StartButton") || other.gameObject.CompareTag("EndButton"))
        {
            touchTime = 0f;
            ResetFillAmount();
        }
        if (!other.gameObject.CompareTag("Target")) return;

        GameManager.IncrementCount();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("StartButton"))
        {
            if (!(Time.time - touchTime > 2f)) return;
            ResetFillAmount();
            scene.OnClickStart();
        }
        else if (other.gameObject.CompareTag("EndButton"))
        {
            if (!(Time.time - touchTime > 2f)) return;
            ResetFillAmount();
            scene.OnClickEnd();
        }
    }
}
