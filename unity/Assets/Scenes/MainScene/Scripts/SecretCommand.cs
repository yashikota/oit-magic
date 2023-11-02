using UnityEngine;

public class SecretCommand : MonoBehaviour
{
    [SerializeField] private Scene scene;
    private readonly string[] secretCode = new string[] { "up", "up", "down", "down", "left", "right", "left", "right", "b", "a" };
    private int index;

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(secretCode[index]))
            {
                index++;
            }
            else
            {
                index = 0;
            }
        }

        if (index != secretCode.Length) return;
        index = 0;
        scene.GameOver();
    }
}
