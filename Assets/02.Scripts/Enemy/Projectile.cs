using MoreMountains.Feedbacks;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    public GameObject ImpactParticle;
    public GameObject ProjectileParticle;
    public GameObject MuzzleParticle;
    public List<GameObject> TrailParticleList;

    private SphereCollider _sphereCollider;
    public LayerMask LayerMask;

    public float MoveSpeed = 5f;
    private Vector3 _direction;
    public float Radius = 0.5f;

    private float _destroyTime = 0f;
    private bool _isDestroyed = false;
    private bool _isReady = false;

    private Damage _damage;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();

        ProjectileParticle = Instantiate(ProjectileParticle, transform.position, transform.rotation);
        ProjectileParticle.transform.parent = transform;

        if (MuzzleParticle)
        {
            MuzzleParticle = Instantiate(MuzzleParticle, transform.position, transform.rotation);
            Destroy(MuzzleParticle, 1.5f);
        }

        _direction = transform.forward;
    }

    public void Init(Damage damage)
    {
        _damage = damage;
        _isReady = true;
    }

    private void FixedUpdate()
    {
        if (_isDestroyed || _isReady == false) return;

        float radius = Radius;

        transform.position += _direction * MoveSpeed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, radius, LayerMask))
        {
            transform.position = hit.point;

            ImpactParticle = Instantiate(ImpactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal));
            if (hit.transform.CompareTag("Player"))
            {
                PlayerManager.Instance.Player.TakeDamage(_damage);
            }

            foreach (GameObject trail in TrailParticleList)
            {
                GameObject currentTrail = transform.Find(ProjectileParticle.name + "/" + trail.name).gameObject;
                currentTrail.transform.parent = null;
                Destroy(currentTrail, 3f);
            }
            Destroy(ProjectileParticle, 3f);
            Destroy(ImpactParticle, 5.0f);
            DestroyMissile();
        }
        else
        {
            _destroyTime += Time.deltaTime;

            if (_destroyTime >= 5f)
            {
                DestroyMissile();
            }
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
        _isReady = false;
        Destroy(gameObject);

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();

        // 0번째는 자기 자신
        for (int i = 1; i < trails.Length; i++)
        {
            ParticleSystem trail = trails[i];
            if (trail.gameObject.name.Contains("Trail"))
            {
                trail.transform.SetParent(null);
                Destroy(trail.gameObject, 2f);
            }
        }
    }
}
