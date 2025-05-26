using UnityEngine;

public class InitRenderTexture : MonoBehaviour
{
    public RenderTexture fogMask;
    void Start()
    {
        ClearFogMask(fogMask);
    }

    void ClearFogMask(RenderTexture fogMask)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = fogMask;
        GL.Clear(true, true, Color.black); // 검정색으로 초기화
        RenderTexture.active = active;
    }

}
