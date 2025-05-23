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

        _direction = new Vector3(PlayerManager.Instance.Player.transform.position.x, transform.position.y, PlayerManager.Instance.Player.transform.position.z) - transform.position;
        _direction = _direction.normalized;
    }

    private void FixedUpdate()
    {
        if (_isDestroyed) return;

        float radius = Radius;

        transform.position += _direction * MoveSpeed * Time.deltaTime;

        RaycastHit hit;
        if(Physics.SphereCast(transform.position, radius, transform.forward, out hit, radius, LayerMask))
        {
            transform.position = hit.point;

            ImpactParticle = Instantiate(ImpactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal));
            if (hit.transform.CompareTag("Player"))
            {
                Damage damage = new Damage();
                damage.From = this.gameObject;
                damage.Value = 10;
                PlayerManager.Instance.Player.TakeDamage(damage);
            }

            foreach(GameObject trail in TrailParticleList)
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

            if(_destroyTime >= 5f)
            {
                DestroyMissile();
            }
        }
    }

    private void DestroyMissile()
    {
        _isDestroyed = true;

        foreach(GameObject trail in TrailParticleList)
        {
            GameObject currentTrail = transform.Find(ProjectileParticle.name + "/" + trail.name).gameObject;
            currentTrail.transform.parent = null;
            Destroy(currentTrail, 3f);
        }
        Destroy(ProjectileParticle, 3f);
        Destroy(gameObject);

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();

        // 0번째는 자기 자신
        for(int i = 1; i < trails.Length; i++)
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
