using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSkillRangeIndicator : MonoBehaviour
{
    [SerializeField] public Vector3 Position;       // 위치
    [SerializeField] public float DistanceRange = 5f;          // 범위
    [SerializeField] public Color Color = Color.red;   // 색상
    [SerializeField] public float InnerRangePercent = 0.5f;
    [SerializeField] public float Direction;      // 방향
    [SerializeField] public float AngleRange = 60f; // 각도 범위
    [SerializeField] public float CastingPercent = 0f;

    [SerializeField] private DecalProjector _projector;   // URP Decal Projector 컴포넌트
    [SerializeField] private Material _materialInstance;  // 인스턴스화된 Material

    private void Awake()
    {
        _projector = GetComponent<DecalProjector>();
        _materialInstance = Instantiate(_projector.material);
        _projector.material = _materialInstance; // DecalProjector에 인스턴스화된 Material 할당
    }

    private void Start()
    {
        InitProperties();
    }

    private void Update()
    {
        transform.position = Position;

        CastingPercent += Time.deltaTime;
        CastingPercent = CastingPercent % 1f;

        _projector.material.SetFloat("_Direction", Direction);
        _projector.material.SetFloat("_CastingPercent", CastingPercent);
    }

    private void InitProperties()
    {
        transform.position = Position;

        _projector.size = new Vector3(DistanceRange, DistanceRange, 10f);
        _materialInstance.SetColor("_BaseColor", Color);
        _materialInstance.SetFloat("_CastPercent", CastingPercent);
        _materialInstance.SetFloat("_InnerRange", InnerRangePercent);
        _materialInstance.SetFloat("_AngleRange", AngleRange);
    }

    private void SetDirection()
    {
    }
}
