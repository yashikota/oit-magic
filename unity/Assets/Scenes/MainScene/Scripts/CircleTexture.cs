using UnityEngine;

public class CircleTexture : MonoBehaviour
{
    public static int textureSize = 256;
    public Texture2D GenerateCircleTexture()
    {
        Texture2D circleTexture = new(textureSize, textureSize)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };

        Vector2 circleCenter = new(textureSize / 2, textureSize / 2);

        for (int x = 0; x < circleTexture.width; x++)
        {
            for (int y = 0; y < circleTexture.height; y++)
            {
                Vector2 pixelPosition = new(x, y);
                float distance = Vector2.Distance(pixelPosition, circleCenter);
                circleTexture.SetPixel(x, y, distance <= textureSize / 2 ? Color.white : Color.clear);
            }
        }

        circleTexture.Apply();

        return circleTexture;
    }
}
