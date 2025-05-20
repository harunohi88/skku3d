using UnityEngine;

public class Lightning : MonoBehaviour
{
    public ObjectPool<Lightning> thisPool;

    private BoxCollider _collider;
    private float _time = 0f;
    private float _castingTime;

    public ParticleSystem LightningStrikeParticle;
    public ParticleSystem LightningSpinParticle;

    private void Awake()
    {
        _collider = gameObject.GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public void Init(float castingTime)
    {
        _castingTime = castingTime;
        _collider.enabled = false;
        BossIndicatorManager.Instance.SetIndicator(transform.position, BossAIManager.Instance.Pattern1Radius, BossAIManager.Instance.Pattern1Radius, 0, 360, 0, castingTime, 0);
        _time = 0f;
        transform.localScale = new Vector3(BossAIManager.Instance.Pattern1Radius, 1, BossAIManager.Instance.Pattern1Radius);
    }

    private void Update()
    {
        if (_collider.enabled)
        {
            _time += Time.deltaTime;
            if (_time >= BossAIManager.Instance.Pattern1LightningLastTime)
            {
                thisPool.Return(this);
            }
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= _castingTime)
            {
                _collider.enabled = true;
                LightningStrikeParticle.Play();
                LightningSpinParticle.Play();
                _time = 0;
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
