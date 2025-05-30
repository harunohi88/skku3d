using UnityEngine;

public class MagicField : MonoBehaviour
{
    public float LastTime;
    private float _radius;
    private float _time;
    private float _timeForTick;
    private float _tick = 0.2f;

    private Damage _damage;
    private bool _isReady;

    private void Awake()
    {
        _radius = GetComponent<SphereCollider>().radius;
        _isReady = false;
    }

    public void Init(Damage damage)
    {
        _damage = damage;
        _isReady = true;
    }

    private void Update()
    {
        if (_isReady == false) return;

        _time += Time.deltaTime;
        if(_time >= LastTime)
        {
            Destroy(this.gameObject);
            return;
        }

        _timeForTick += Time.deltaTime;
        if(_timeForTick >= _tick)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, LayerMask.GetMask("Enemy"));

            for(int i = 0; i < colliders.Length; i++)
            {
                Damage newDamage = new Damage();
                newDamage.Value = _damage.Value;
                newDamage.From = _damage.From;
                RuneManager.Instance.CheckCritical(ref newDamage);
                colliders[i].GetComponent<AEnemy>()?.TakeDamage(newDamage);
            }

            _timeForTick = 0;
        }
    }
}
