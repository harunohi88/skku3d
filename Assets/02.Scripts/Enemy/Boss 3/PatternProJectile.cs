using MoreMountains.Feedbacks;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Pool;

public class PatternProJectile : MonoBehaviour
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
    private GameObject _currentProjectileParticle;
    private GameObject _currentMuzzleParticle;
    private List<GameObject> _currentTrailParticles = new List<GameObject>();

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        Reset();
    }

    private void Reset()
    {
        _destroyTime = 0f;
        _isDestroyed = false;
        _isReady = false;
        _currentTrailParticles.Clear();

        // Initialize particle pools
        ParticlePool.Instance.InitializePool(ProjectileParticle, "ProjectileParticle", 130);
        if (MuzzleParticle)
        {
            ParticlePool.Instance.InitializePool(MuzzleParticle, "MuzzleParticle", 130);
        }
        if (ImpactParticle)
        {
            ParticlePool.Instance.InitializePool(ImpactParticle, "ImpactParticle", 130);
        }
        foreach (var trail in TrailParticleList)
        {
            ParticlePool.Instance.InitializePool(trail, "Trail_" + trail.name, 130);
        }

        // Get particles from pool
        _currentProjectileParticle = ParticlePool.Instance.GetParticle("ProjectileParticle");
        _currentProjectileParticle.transform.position = transform.position;
        _currentProjectileParticle.transform.rotation = transform.rotation;
        _currentProjectileParticle.transform.parent = transform;

        if (MuzzleParticle)
        {
            _currentMuzzleParticle = ParticlePool.Instance.GetParticle("MuzzleParticle");
            _currentMuzzleParticle.transform.position = transform.position;
            _currentMuzzleParticle.transform.rotation = transform.rotation;
            StartCoroutine(ReturnMuzzleParticleAfterDelay(1.5f));
        }

        _direction = transform.forward;
    }

    private System.Collections.IEnumerator ReturnMuzzleParticleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_currentMuzzleParticle != null)
        {
            ParticlePool.Instance.ReturnParticle("MuzzleParticle", _currentMuzzleParticle);
            _currentMuzzleParticle = null;
        }
    }

    public void Init(Damage damage)
    {
        _damage = damage;
        _isReady = true;
    }

    private void FixedUpdate()
    {
        //if (_isDestroyed) return;

        float radius = Radius;

        transform.position += _direction * MoveSpeed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, radius, LayerMask))
        {
            transform.position = hit.point;

            var impactParticle = ParticlePool.Instance.GetParticle("ImpactParticle");
            impactParticle.transform.position = hit.point;
            impactParticle.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            StartCoroutine(ReturnParticleAfterDelay("ImpactParticle", impactParticle, 5.0f));

            foreach (GameObject trail in TrailParticleList)
            {
                GameObject currentTrail = transform.Find(_currentProjectileParticle.name + "/" + trail.name).gameObject;
                currentTrail.transform.parent = null;
                _currentTrailParticles.Add(currentTrail);
                StartCoroutine(ReturnParticleAfterDelay("Trail_" + trail.name, currentTrail, 3f));
            }

            StartCoroutine(ReturnParticleAfterDelay("ProjectileParticle", _currentProjectileParticle, 3f));
            DestroyMissile();
        }
        else
        {
            _destroyTime += Time.deltaTime;

            if (_destroyTime >= 5f)
            {
                //DestroyMissile();
            }
        }
    }

    private System.Collections.IEnumerator ReturnParticleAfterDelay(string poolName, GameObject particle, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (particle != null)
        {
            ParticlePool.Instance.ReturnParticle(poolName, particle);
        }
    }

    private void DestroyMissile()
    {
        _isDestroyed = true;

        foreach (var trail in _currentTrailParticles)
        {
            if (trail != null)
            {
                trail.transform.parent = null;
            }
        }
        _currentTrailParticles.Clear();

        _isReady = false;
        ProjectilePool.Instance.Return(this);

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
        for (int i = 1; i < trails.Length; i++)
        {
            ParticleSystem trail = trails[i];
            if (trail.gameObject.name.Contains("Trail"))
            {
                trail.transform.SetParent(null);
                StartCoroutine(ReturnParticleAfterDelay("Trail_" + trail.gameObject.name, trail.gameObject, 2f));
            }
        }
    }
}
