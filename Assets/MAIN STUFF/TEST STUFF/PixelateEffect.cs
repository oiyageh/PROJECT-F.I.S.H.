using UnityEngine;

[ExecuteInEditMode]
public class PixelateEffect : MonoBehaviour
{
    public Material pixelMaterial;
    [Range(8, 512)]
    public int pixelSize = 64;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pixelMaterial != null)
        {
            pixelMaterial.SetFloat("_PixelSize", pixelSize);
            Graphics.Blit(src, dest, pixelMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}