using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(BossAIManager))]
public class Boss_MechanicGolem : AEnemy, ISpecialAttackable
{
    public Collider WeaponCollider;
    public Lightning LightningPrefab;
    private ObjectPool<Lightning> _lightningPool;
    private int _attackCount = 0;

    protected override void Start()
    {
        base.Start();
        _lightningPool = new ObjectPool<Lightning>(LightningPrefab, 20, GameObject.FindGameObjectWithTag("Pool").transform);
    }

    public override void Init()
    {
        base.Init();
        _stateMachine.ChangeState(new BossIdleState());
    }

    public override void TakeDamage(Damage damage)
    {
        // 구현 예정
    }

    public override void Attack()
    {
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = false;
    }

    public void OnBaseAttackEnd()
    {
        WeaponCollider.enabled = false;
        _attackCount++;
        if(_attackCount >= 4)
        {
            _attackCount = 0;
            BossAIManager.Instance.SetLastFinishedTime(0, Time.time);
            OnAnimationEnd();
        }
    }

    public void SpecialAttack_01()
    {
        var patternData = BossAIManager.Instance.GetPatternData(1);
        if (patternData != null)
        {
            ClusterInstantiate(
                transform.position, 
                patternData.MaxCount,
                patternData.Range,
                patternData.Range
            );
        }
    }

    public void OnSpecialAttack01End()
    {
        BossAIManager.Instance.SetLastFinishedTime(1, Time.time);
    }

    public void SpecialAttack_02()
    {
        if (_attackCount >= 2) _attackCount = 0;
        if (_attackCount == 0)
        {
            WeaponCollider.enabled = true;
            EnemyRotation.IsFound = false;
        }
        else if(_attackCount == 1)
        {
            // 직선공격 ㄱㄱ
        }
    }

    public void OnSpecialAttack02End()
    {
        _attackCount++;
        if(_attackCount >= 2)
        {
            WeaponCollider.enabled = false;
            BossAIManager.Instance.SetLastFinishedTime(2, Time.time);
            OnAnimationEnd();
        }
        else
        {
            EnemyRotation.IsFound = true;
            WeaponCollider.enabled = false;
        }
    }

    public void SpecialAttack_03()
    {
        Vector3 position = transform.position;
        StartCoroutine(SpecialAttack03_Coroutine(position, transform.forward));
    }

    public IEnumerator SpecialAttack03_Coroutine(Vector3 position, Vector3 forward)
    {
        var patternData = BossAIManager.Instance.GetPatternData(3);
        if (patternData == null) yield break;

        List<SkillIndicator> indicatorList = new List<SkillIndicator>();
        float castingTime = patternData.CastingTime / 3;

        for (int i = 0; i < 3; i++)
        {
            float size = ((i + 1) / 3.0f) * patternData.Range;
            float innerRange = i / (float)(i + 1);
            indicatorList.Add(BossIndicatorManager.Instance.SetCircularIndicator(
                position, 
                size, 
                size, 
                0, 
                patternData.Angle, 
                innerRange, 
                castingTime, 
                0, 
                false
            ));

            Quaternion rotation = Quaternion.LookRotation(forward);
            Vector3 fixedEuler = new Vector3(90f, 0f, -rotation.eulerAngles.y);
            indicatorList[i].transform.rotation = Quaternion.Euler(fixedEuler);
        }

        for(int i = 0; i < 3; i++)
        {
            indicatorList[i].Ready(castingTime);
            yield return new WaitForSeconds(castingTime);

            float radius = ((i + 1) / 3.0f) * patternData.Range / 2;
            List<Collider> colliderList = Physics.OverlapSphere(position, radius, LayerMask).ToList();

            GameObject playerObject = colliderList.Find(x => x.CompareTag("Player"))?.gameObject;

            if (playerObject)
            {
                Vector3 directionToTarget = (playerObject.transform.position - position).normalized;
                if(Vector3.Dot(forward, directionToTarget) > Mathf.Cos(patternData.Angle))
                {
                    float distance = Vector3.Distance(playerObject.transform.position, position);
                    if(distance > (i / (float)(i + 1)) * radius)
                    {
                        Debug.Log("Pattern 3 데미지 발생");
                    }
                }
            }
        }
    }

    public void OnSpecialAttack03End()
    {
        BossAIManager.Instance.SetLastFinishedTime(3, Time.time);
        EnemyRotation.IsFound = true;
    }

    public void SpecialAttack_04()
    {
        // 구현 예정
    }

    public void OnSpecialAttack04End()
    {
        BossAIManager.Instance.SetLastFinishedTime(4, Time.time);
    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(BossAIManager.Instance.DecideNextState());
    }

    private void ClusterInstantiate(Vector3 center, int count, float radiusX, float radiusY)
    {
        int placed = 0;
        var patternData = BossAIManager.Instance.GetPatternData(1);

        while(placed < count)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Mathf.Sqrt(Random.Range(0, 1f));

            float x = Mathf.Cos(angle) * radius * radiusX;
            float y = Mathf.Sin(angle) * radius * radiusY;

            Vector3 spawnPoint = center + new Vector3(x, center.y, y);
            Lightning lightning = _lightningPool.Get();
            lightning.transform.position = spawnPoint;
            lightning.Init(patternData.CastingTime);
            if (lightning.thisPool == null) lightning.thisPool = _lightningPool;

            placed++;
        }
    }
}
