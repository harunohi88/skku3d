using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SkillIndicator : MonoBehaviour
{
    public ObjectPool<SkillIndicator> thisPool;

    private DecalProjector _projector;
    private Material _instancedMaterial;

    private float percent = 0f;
    private bool IsReady = false;
    private float _castingTime = 0f;
    private float _time = 0f;

    private void Awake()
    {
        _projector = GetComponent<DecalProjector>();
        _instancedMaterial = Instantiate(_projector.material);
        _projector.material = _instancedMaterial;
        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    public void Init(float width, float height, float direction, float angleRange, float innerRange, float castingPercent, ObjectPool<SkillIndicator> pool = null)
    {
        _projector.size = new Vector3(width, height, 1);
        _time = 0f;

        _instancedMaterial.SetFloat("_Direction", direction);
        _instancedMaterial.SetFloat("_AngleRange", angleRange);
        _instancedMaterial.SetFloat("_InnerRange", innerRange);
        _instancedMaterial.SetFloat("_CastingPercent", castingPercent);
        thisPool = pool;

        percent = 0;
    }

    public void Ready(float castingTime)
    {
        _castingTime = castingTime;
        IsReady = true;
    }

    private void Update()
    {
        if (IsReady)
        {
            _time += Time.deltaTime;
            percent = _time / _castingTime;
            _instancedMaterial.SetFloat("_CastingPercent", percent);

            if(percent >= 1f)
            {
                StopIndicator();
            }
        }
    }

    public void StopIndicator()
    {
        if (thisPool != null) thisPool.Return(this);
        else Destroy(this.gameObject);
    }
}
