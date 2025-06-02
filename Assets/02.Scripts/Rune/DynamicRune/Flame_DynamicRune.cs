using System.Collections.Generic;
using UnityEngine;

public class Flame_DynamicRune : ADynamicRuneObject
{
    public GameObject HitObject;
    public GameObject FloorObject;

    private SphereCollider _sphereCollider;
    public LayerMask LayerMask;

    public float MoveSpeed = 5f;
    private Vector3 _direction;
    private Vector3 targetPosition;
    public float Radius = 0.5f;

    private float _destroyTime = 0f;
    private bool _isDestroyed = false;
    private bool _isReady = false;

    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
        _sphereCollider = GetComponent<SphereCollider>();

        Radius = _sphereCollider.radius;
        _direction = transform.forward;
        MoveSpeed = moveSpeed;

        _isDestroyed = false;
        _isReady = true;
        RaycastHit hit;
        Physics.Raycast(transform.position, _direction, out hit, 100f, LayerMask);
        targetPosition = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);
    }

    public override void Update()
    {
    }

    private void FixedUpdate()
    {
        if (_isDestroyed || _isReady == false) return;
        transform.position += _direction * MoveSpeed * Time.deltaTime;

        if (Vector3.Distance(targetPosition, transform.position) <= 0.4f)
        {
            Instantiate(HitObject, transform.position, Quaternion.identity);

            Collider[] colliders = Physics.OverlapSphere(transform.position, Radius, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < colliders.Length; i++)
            {
                Damage newDamage = new Damage();
                newDamage.Value = _damage.Value;
                newDamage.From = _damage.From;
                RuneManager.Instance.CheckCritical(ref newDamage);
                colliders[i].GetComponent<AEnemy>()?.TakeDamage(newDamage);
            }
  
            MagicField floor = Instantiate(FloorObject, targetPosition, Quaternion.identity).GetComponent<MagicField>();
            Damage floorDamage = new Damage();
            floorDamage.Value = _damage.Value / 3;
            floorDamage.From = _damage.From;
            floor.Init(floorDamage);

            _isDestroyed = true;
            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
        }
    }
}
