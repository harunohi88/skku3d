using UnityEngine;

public class Lightning : MonoBehaviour
{
    public ObjectPool<Lightning> thisPool;

    private BoxCollider _collider;
    private float _time = 0f;
    private float _tickTime = 0f;
    private float _castingTime;
    private float _duration;
    private bool _isLightnigOn = false;

    public ParticleSystem LightningStrikeParticle;
    public ParticleSystem LightningSpinParticle;

    private void Awake()
    {
        _collider = gameObject.GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public void Init(float castingTime, float radius, float duration)
    {
        _castingTime = castingTime;
        _collider.enabled = false;
        _duration = duration;
        _isLightnigOn = false;
        _tickTime = 0f;
        BossIndicatorManager.Instance.SetCircularIndicator(transform.position, radius, radius, 0, 360, 0, castingTime, 0, Color.red);
        _time = 0f;
        transform.localScale = new Vector3(radius, 1, radius);
    }

    private void Update()
    {
        if (_isLightnigOn)
        {
            _time += Time.deltaTime;
            if (_time >= _duration)
            {
                thisPool.Return(this);
                return;
            }

            _tickTime += Time.deltaTime;
            if(_tickTime >= 0.5f)
            {
                _collider.enabled = false;
                _collider.enabled = true;
                _tickTime = 0f;
            }
        }

        if (_isLightnigOn == false)
        {
            _time += Time.deltaTime;
            if (_time >= _castingTime)
            {
                _collider.enabled = true;
                LightningStrikeParticle.Play();
                LightningSpinParticle.Play();
                _time = 0;
                _isLightnigOn = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("데미지 줌");
        }
    }
}
