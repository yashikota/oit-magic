using UnityEngine;

public class GenerateGrass : MonoBehaviour
{
    [SerializeField] GameObject grassPrefab;

    void Start()
    {
        for (float x = -100; x <= 100; x += 0.75f)
        {
            for (float z = -10; z <= 75; z += 0.75f)
            {
                var grass = Instantiate(grassPrefab, transform);
                grass.transform.position = new Vector3(x, 0, z);
            }
        }
    }
}
