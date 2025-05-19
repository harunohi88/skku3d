using System.Collections.Generic;
using UnityEngine;

// 플레이어와 일정 거리 가까워지면 스폰 - 풀링 하면 굳이? - 플레이어매니저가 플레이어 가지고 있어서 transform 받으면 됨
// 죽었던 애들은 그대로 다시 스폰 - 일정시간 뒤에?
// 
// 


public class EnemySpawner : MonoBehaviour
{
    public Transform SpawnPosition;
    public List<AEnemy> EnemyList;
    //public List<EliteEnemy> EliteEnemyList;
    public int EnemySpawnCount = 10;
    public int EliteEnemySpawnCount = 3;
    
    public float DetectPlayerRange = 20f;

    [SerializeField]
    private float _spawnRadius = 10f;
    [SerializeField]
    private float _eliteSpawnRate = 0.1f;
    private float _eliteSpawnRateIncrease = 0.1f;

    private bool _isPlayerInRangeOnce = false;
    [SerializeField]
    private int _activedEnemy;

    public GameObject Terget;

    private void Update()
    {
        // 테스트용
        if(Input.GetKeyDown(KeyCode.A))
        {
            ActivedEnemyCountDecrease();
        }

        // 방문한적이 없을 때
        if(!_isPlayerInRangeOnce)
        {   /* 실제 사용
            // 범위 내에 들어오면 활성화 해준다.
            if(Vector3.Distance(transform.position, PlayerManager.Instance.Player.transform.position) <= DetectPlayerRange)
            {
                _isPlayerInRangeOnce = true;
                SummonEnemy();
            }*/
            // 테스트용
            if(Vector3.Distance(transform.position, Terget.transform.position) <= DetectPlayerRange)
            {
                _isPlayerInRangeOnce = true;
                SummonEnemy();
            }
        }
        else
        {
            ResetPlayerInRangeOnce();
        }
    }

    private void ResetPlayerInRangeOnce()
    {
        if(_activedEnemy <= 0)
        {
            _isPlayerInRangeOnce = false;
            EnemyDisable();
        }
    }

    private void SummonEnemy()
    {
        EnemyList.Clear();
        for(int i=0; i<EnemySpawnCount; i++)
        {
            // TODO: 엘리트 에너미도 풀에서 받아와서 리스트에 추가하기
            // 확률 적으로 basic이냐 엘리트냐 받아온다
            var enemy = BasicEnemyPool.Instance.Get();
            EnemyList.Add(enemy);
        }
        
        _activedEnemy = EnemySpawnCount;

        ResetPosition();
    }

    private void ResetPosition()
    {
        foreach(var enemy in EnemyList)
        {
            // TODO: 스폰 포인트에 소환 - 주위 원 반경에 소환
            Vector3 posRandOnSpherePos = Random.onUnitSphere * _spawnRadius;
            posRandOnSpherePos.y = SpawnPosition.position.y;

            Vector3 rotRandOnSpherePos = Random.onUnitSphere * 100f;
            rotRandOnSpherePos.x = 0;
            rotRandOnSpherePos.z = 0;

            enemy.transform.position = posRandOnSpherePos;
            enemy.transform.rotation = Quaternion.Euler(rotRandOnSpherePos); 
        }
        // TODO: 엘리트 에너미도 추가
    }

    private void EnemyDisable()
    {
        foreach(var enemy in EnemyList)
        {
            // TODO : 다시 풀로 Return해준다.
        }
        EnemyList.Clear();
    }

    public void EliteSpawnRateIncrease()
    {
        // TODO : 엘리트 스폰 확률 증가
        _eliteSpawnRate += _eliteSpawnRateIncrease;
    }

    public void ActivedEnemyCountDecrease()
    {
        _activedEnemy--;
    }


    /*
    private int GetActiveEnemyCount()
    {
        int count = 0;
        foreach (var enemy in BasicEnemyList)
        {
            if (enemy.gameObject.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
    */
}
