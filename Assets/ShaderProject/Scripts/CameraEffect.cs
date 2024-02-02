using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraEffect : MonoBehaviour
{
    [SerializeField] Material _Material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_Material == null)
        {
            Graphics.Blit(source, destination);
            return;
        }
        
        Graphics.Blit(source, destination, _Material);
    }

    public void SetMaterial(Material material) => _Material = material;
}
