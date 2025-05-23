using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SkillIndicator : MonoBehaviour
{
    public ObjectPool<SkillIndicator> thisPool;
    [SerializeField] private Material CircularMaterial;
    [SerializeField] private Material SquareMaterial;

    private DecalProjector _projector;
    private Material _instancedMaterial;

    private float percent = 0f;
    private bool _isReady = false;
    private float _castingTime = 0f;
    private float _time = 0f;

    private void Awake()
    {
        _projector = GetComponent<DecalProjector>();
        CircularMaterial = Instantiate(CircularMaterial);
        SquareMaterial = Instantiate(SquareMaterial);

        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    public void CircularInit(float width, float height, float direction, float angleRange, float innerRange, float castingPercent, ObjectPool<SkillIndicator> pool = null)
    {
        _projector.material = CircularMaterial;
        _instancedMaterial = _projector.material;
        _projector.size = new Vector3(width, height, _projector.size.z);
        _time = 0f;
        _isReady = false;
        _instancedMaterial.SetFloat("_Direction", direction);
        _instancedMaterial.SetFloat("_AngleRange", angleRange);
        _instancedMaterial.SetFloat("_InnerRange", innerRange);
        _instancedMaterial.SetFloat("_CastingPercent", castingPercent);
        if(pool != null) thisPool = pool;
        transform.eulerAngles = new Vector3(90, 0, 0);

        percent = 0;
    }

    public void SquareInit(float width, float height, float direction, float innerRange, float castingPercent, ObjectPool<SkillIndicator> pool = null)
    {
        _projector.material = SquareMaterial;
        _instancedMaterial = _projector.material;
        _projector.size = new Vector3(width, height, _projector.size.z);
        _time = 0f;
        _isReady = false;
        _instancedMaterial.SetFloat("_Direction", direction);
        _instancedMaterial.SetFloat("_InnerRange", innerRange);
        _instancedMaterial.SetFloat("_CastingPercent", castingPercent);
        if (pool != null) thisPool = pool;
        transform.eulerAngles = new Vector3(90, 0, 0);

        percent = 0;
    }

    public void Ready(float castingTime)
    {
        _castingTime = castingTime;
        _isReady = true;
    }

    private void Update()
    {
        if (_isReady)
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

    public void SetPosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, 2f, position.z);
    }
}
