using UnityEngine;

[ExecuteInEditMode]
public class CameraMaterial : MonoBehaviour
{
	
	public Material material;
	
	void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, material);
    }
}
