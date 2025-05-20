using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CircleIndicator : MonoBehaviour
{
    public DecalProjector DecalProjector;

    private Material _material;

    private void Start()
    {
        DecalProjector = GetComponent<DecalProjector>();
        _material = Instantiate(DecalProjector.material);
        DecalProjector.material = _material;
    }

    public void SetRange(float Range)
    {
        Vector3 currentSize = DecalProjector.size;
        DecalProjector.size = new Vector3(Range, Range, currentSize.z);
    }
}
