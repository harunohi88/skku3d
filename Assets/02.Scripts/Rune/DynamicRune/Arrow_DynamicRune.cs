using UnityEngine;

public class Arrow_DynamicRune : ADynamicRuneObject
{
    public override void Init(Damage damage, float radius, float approachDuration, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, approachDuration, startPosition, targetTransform, TID);
    }

    public override void Update()
    {
        _time += Time.deltaTime / _approachDuration;
        transform.position = GetQuadraticBezierPoint(_time, _startPosition, _controlPoint, _targetTransform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == _targetTransform.gameObject.GetInstanceID())
        {
            other.GetComponent<AEnemy>().TakeDamage(_damage);

            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
        }
    }
}
