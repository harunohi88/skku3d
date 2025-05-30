using UnityEngine;

public class Explosion_DynamicRune : ADynamicRuneObject
{
    private float _duration = 1f;
    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
    }

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, LayerMask.GetMask("Enemy"));
        for(int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetInstanceID() == _targetTransform.gameObject.GetInstanceID()) continue;
            Damage newDamage = new Damage();
            newDamage.Value = _damage.Value;
            newDamage.From = _damage.From;
            RuneManager.Instance.CheckCritical(ref newDamage);

            colliders[i].GetComponent<AEnemy>().TakeDamage(newDamage);
        }
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if(_time >= _duration)
        {
            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
        }
    }
}
