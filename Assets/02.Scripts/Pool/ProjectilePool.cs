using UnityEngine;
using System.Collections.Generic;

namespace Pool
{
    public class ProjectilePool : BehaviourSingleton<ProjectilePool>
    {
        private ObjectPool<PatternProJectile> _pool;
        public PatternProJectile ProjectilePrefab;
        public int PoolSize = 130;

        private void Awake()
        {
            _pool = new ObjectPool<PatternProJectile>(ProjectilePrefab, PoolSize, transform);
        }

        public PatternProJectile Get()
        {
            var projectile = _pool.Get();
            // Reset projectile state
            projectile.transform.position = Vector3.zero;
            projectile.transform.rotation = Quaternion.identity;
            projectile.transform.localScale = Vector3.one;
            projectile.gameObject.SetActive(true);
            return projectile;
        }

        public void Return(PatternProJectile projectile)
        {
            if (projectile != null)
            {
                projectile.gameObject.SetActive(false);
                _pool.Return(projectile);
            }
        }
    }
} 