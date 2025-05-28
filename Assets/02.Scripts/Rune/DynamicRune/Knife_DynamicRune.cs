using UnityEngine;

public class Knife_DynamicRune : ADynamicRuneObject
{
    private bool _isStabbing;
    public int MaxStabCount = 3;
    private int StabCount = 0;
    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
        _isStabbing = false;
        StabCount = 0;
        _time = 0;
    }

    public override void Update()
    {
        if(_isStabbing == false)
        {
            float moveStep = _moveSpeed * Time.deltaTime;
            float timeStep = moveStep / _bezierLength;
            _time += timeStep;
            transform.position = GetQuadraticBezierPoint(_time, _startPosition, _controlPoint, _targetTransform.position);

            if(_time >= 1f)
            {
                _isStabbing = true;
                _time = 0f;
            }
        }
        else
        {
            _time += Time.deltaTime * 2f;
            float angle = _time * Mathf.PI * 2;
            float x = Mathf.Sin(angle);
            float y = Mathf.Sin(angle) * Mathf.Cos(angle);
            Vector3 offset = new Vector3(x * 2f, 0, y * 2f);
            transform.position = _targetTransform.position + offset;
        }

        if(_targetTransform.gameObject.activeSelf == false) RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetInstanceID() == _targetTransform.gameObject.GetInstanceID())
        {
            other.GetComponent<AEnemy>().TakeDamage(_damage);
            StabCount++;

            if(StabCount >= MaxStabCount)
            {
                RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
            }
        }
    }
}
