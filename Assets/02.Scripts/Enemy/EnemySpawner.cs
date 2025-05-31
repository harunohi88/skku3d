using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 플레이어와 일정 거리 가까워지면 스폰 - 풀링 하면 굳이? - 플레이어매니저가 플레이어 가지고 있어서 transform 받으면 됨
// 죽었던 애들은 그대로 다시 스폰 - 일정시간 뒤에?
// 
// 


public class EnemySpawner : MonoBehaviour
{
    public Transform SpawnPosition;
    public const int OriginEnemySpawnCount = 10;
    public int EliteEnemySpawnCount = 3;
    
    public float DetectPlayerRange = 40f;


    private int EnemySpawnCount;
    [SerializeField]
    private float _spawnRadius = 5f;
    [SerializeField]
    private float _eliteSpawnRate = 0.1f;
    private float _eliteSpawnRateIncrease = 0.1f;

    private bool _isPlayerInRangeOnce = false;
    [SerializeField]
    private int _activedEnemy;

    private void ResetPlayerInRangeOnce()
    {
        _isPlayerInRangeOnce = false;
    }

    private void SummonEnemy()
    {
        EnemySpawnCount = (int)(OriginEnemySpawnCount * TimeManager.Instance.DifficultyMultiplier.EnemyCountMultiplier);
        for(int i=0; i<EnemySpawnCount; i++)
        {
            // TODO: 엘리트 에너미도 풀에서 받아와서 리스트에 추가하기
            // 확률 적으로 basic이냐 엘리트냐 받아온다
            if(Random.Range(0f, 1f) < _eliteSpawnRate)
            {
                var enemy = EliteEnemyPool.Instance.Get();

                enemy.MaxHealth = GameManager.Instance.GetEnemyBaseHealth(enemy.Type);
                enemy.Damage = GameManager.Instance.GetEnemyBaseDamage(enemy.Type);
                enemy.Init(this);
                EnemyTracker.Register(enemy.transform);

                enemy.Agent.enabled = false;
                ResetPosition(enemy.gameObject);
                enemy.Agent.enabled = true;
            }
            else
            {
                var enemy = BasicEnemyPool.Instance.Get();

                enemy.MaxHealth = GameManager.Instance.GetEnemyBaseHealth(enemy.Type);
                enemy.Damage = GameManager.Instance.GetEnemyBaseDamage(enemy.Type);
                enemy.Init(this);
                EnemyTracker.Register(enemy.transform);

                enemy.Agent.enabled = false;
                ResetPosition(enemy.gameObject);
                enemy.Agent.enabled = true;
            }
        }
        
        _activedEnemy = EnemySpawnCount;

    }

    private void ResetPosition(GameObject enemy)
    {
        Vector3 posRandOnSpherePos = SpawnPosition.position + Random.onUnitSphere * _spawnRadius;
        posRandOnSpherePos.y = SpawnPosition.position.y;

        Vector3 rotRandOnSpherePos = Random.onUnitSphere * 100f;
        rotRandOnSpherePos.x = 0;
        rotRandOnSpherePos.z = 0;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(posRandOnSpherePos, out hit, 10.0f, NavMesh.AllAreas))
        {
            enemy.transform.position = hit.position;
            //enemy.transform.rotation = Quaternion.Euler(rotRandOnSpherePos);
        }
    }

    public void EliteSpawnRateIncrease()
    {
        // 엘리트 스폰 확률 증가
        _eliteSpawnRate += _eliteSpawnRateIncrease;
    }

    public void ActivedEnemyCountDecrease()
    {
        _activedEnemy--;

        if(_activedEnemy <= 0) ResetPlayerInRangeOnce();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isPlayerInRangeOnce)
            {
                _isPlayerInRangeOnce = true;
                SummonEnemy();
            }
        }
    }
}
