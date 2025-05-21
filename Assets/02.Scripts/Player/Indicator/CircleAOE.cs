using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CircleAOE : MonoBehaviour
{
    public DecalProjector DecalProjector;

    private Material _material;

    private void Awake()
    {
        DecalProjector = GetComponent<DecalProjector>();
        _material = Instantiate(DecalProjector.material);
        DecalProjector.material = _material;
    }

    public void SetColor(Color color)
    {
        _material.color = color;
    }

    public void SetDirection(float direction)
    {
        _material.SetFloat("_Direction", direction);
    }
    
    public void SetAngle(float angle)
    {
        _material.SetFloat("_AngleRange", angle);
    }

    public void SetInnerRadius(float radius)
    {
        _material.SetFloat("_InnerRange", radius);
    }

    public void SetCastingPercentage(float percentage)
    {
        _material.SetFloat("_CastingPercent", percentage);
    }

    public void SetSize(float size)
    {
        Vector3 originalSize = DecalProjector.size;
        DecalProjector.size = new Vector3(size, size, originalSize.z);
    }
}
