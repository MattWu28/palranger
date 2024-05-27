using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour
{
    private Material transitionMaterial;

    public void SetTransitionMaterial(Material material)
    {
        transitionMaterial = material;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (transitionMaterial != null)
            Graphics.Blit(src, dst, transitionMaterial);
    }
}
