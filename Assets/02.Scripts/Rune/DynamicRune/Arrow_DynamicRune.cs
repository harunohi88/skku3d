using UnityEngine;

public class Arrow_DynamicRune : ADynamicRuneObject
{
    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
    }

    public override void Update()
    {
        float moveStep = _moveSpeed * Time.deltaTime;
        float timeStep = moveStep / _bezierLength;
        _time += timeStep;
        transform.position = GetQuadraticBezierPoint(_time, _startPosition, _controlPoint, _targetTransform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == _targetTransform.gameObject.GetInstanceID())
        {
            Damage newDamage = new Damage();
            newDamage.Value = _damage.Value;
            newDamage.From = _damage.From;
            RuneManager.Instance.CheckCritical(ref newDamage);

            other.GetComponent<AEnemy>().TakeDamage(newDamage);

            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
        }
    }
}
