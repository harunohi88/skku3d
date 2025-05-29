using System.Collections.Generic;
using UnityEngine;

public class Flame_DynamicRune : ADynamicRuneObject
{
    public GameObject ImpactParticlePrefab;
    public GameObject ProjectileParticlePrefab;
    public GameObject MuzzleParticlePrefab;

    public GameObject ImpactParticle;
    public GameObject ProjectileParticle;
    public GameObject MuzzleParticle;
    public List<GameObject> TrailParticleList;

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

        ProjectileParticle = Instantiate(ProjectileParticlePrefab, transform.position, transform.rotation);
        ProjectileParticle.transform.parent = transform;

        if (MuzzleParticle)
        {
            MuzzleParticle = Instantiate(MuzzleParticlePrefab, transform.position, transform.rotation);
            Destroy(MuzzleParticle, 1.5f);
        }

        Radius = _sphereCollider.radius;
        _direction = transform.forward;
        MoveSpeed = moveSpeed;

        _isReady = true;
        RaycastHit hit;
        Physics.Raycast(transform.position, _direction, out hit, 100f, LayerMask);
        targetPosition = new Vector3(hit.point.x, hit.point.y + 0.3f, hit.point.z);
    }

    public override void Update()
    {
        // 화면 밖에서 안으로 떨어짐
    }

    private void FixedUpdate()
    {
        if (_isDestroyed || _isReady == false) return;
        transform.position += _direction * MoveSpeed * Time.deltaTime;

        if (Vector3.Distance(targetPosition, transform.position) <= 0.5f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Radius, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < colliders.Length; i++)
            {
                Damage newDamage = new Damage();
                newDamage.Value = _damage.Value;
                newDamage.From = _damage.From;
                RuneManager.Instance.CheckCritical(ref newDamage);
                colliders[i].GetComponent<AEnemy>().TakeDamage(newDamage);
            }

            MagicField floor = Instantiate(FloorObject, targetPosition, Quaternion.identity).GetComponent<MagicField>();
            Damage floorDamage = new Damage();
            floorDamage.Value = _damage.Value / 3;
            floorDamage.From = _damage.From;
            floor.Init(floorDamage);

            foreach (GameObject trail in TrailParticleList)
            {
                GameObject currentTrail = transform.Find(ProjectileParticle.name + "/" + trail.name).gameObject;
                currentTrail.transform.parent = null;
                Destroy(currentTrail, 3f);
            }
            Destroy(ProjectileParticle, 3f);
            Destroy(ImpactParticle, 5.0f);
            DestroyMissile();

            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, Radius); // 시작 지점

    //    Vector3 end = transform.position + _direction.normalized * Radius;
    //    Gizmos.DrawWireSphere(end, Radius);     // 끝 지점

    //    // 본체 라인
    //    Gizmos.DrawLine(transform.position + Vector3.right * Radius, end + Vector3.right * Radius);
    //    Gizmos.DrawLine(transform.position + Vector3.up * Radius, end + Vector3.up * Radius);
    //    Gizmos.DrawLine(transform.position - Vector3.up * Radius, end - Vector3.up * Radius);
    //}

    private void DestroyMissile()
    {
        _isDestroyed = true;

        foreach (GameObject trail in TrailParticleList)
        {
            GameObject currentTrail = transform.Find(ProjectileParticle.name + "/" + trail.name).gameObject;
            currentTrail.transform.parent = null;
            Destroy(currentTrail, 3f);
        }
        Destroy(ProjectileParticle, 3f);
        Destroy(gameObject);

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();

        // 0번째는 자기 자신
        for (int i = 1; i < trails.Length; i++)
        {
            ParticleSystem trail = trails[i];
            if (trail.gameObject.name.Contains("Trail"))
            {
                trail.transform.SetParent(null);
                _isReady = false;
                Destroy(trail.gameObject, 2f);
            }
        }
    }
}
