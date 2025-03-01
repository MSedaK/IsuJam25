using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AssignRenderTexture : MonoBehaviour
{
    public RenderTexture rt;
    public RenderTexture rt2;
    public RenderTexture rt3;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.targetTexture = rt;
    }
}
