using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AssignRenderTexture : MonoBehaviour
{
    public RenderTexture rt;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.targetTexture = rt;
    }
}
