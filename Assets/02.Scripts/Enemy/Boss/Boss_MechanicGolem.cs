using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(BossAIManager))]
public class Boss_MechanicGolem : AEnemy
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
            BossAIManager.Instance.LastFinishedtimeList[0] = Time.time;
            OnAnimationEnd();
        }
    }

    public void SpecialAttack_01()
    {
        ClusterInstantiate(transform.position, 20, BossAIManager.Instance.Pattern1Range, BossAIManager.Instance.Pattern1Range);
    }

    public void OnSpecialAttack01End()
    {
        BossAIManager.Instance.LastFinishedtimeList[1] = Time.time;
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
            BossAIManager.Instance.LastFinishedtimeList[2] = Time.time;
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
        List<SkillIndicator> indicatorList = new List<SkillIndicator>();
        float castingTime = BossAIManager.Instance.PatternCastingtimeList[3] / 3;

        for (int i = 0; i < 3; i++)
        {
            float size = ( (i + 1) / 3.0f ) * BossAIManager.Instance.Pattern3Range;
            float innerRange = i / (float)(i + 1);
            indicatorList.Add(BossIndicatorManager.Instance.SetIndicator(position, size, size, 0, BossAIManager.Instance.Pattern3Angle, innerRange, castingTime, 0, false));

            Quaternion rotation = Quaternion.LookRotation(forward);

            Vector3 fixedEuler = new Vector3(90f, 0f, -rotation.eulerAngles.y);
            indicatorList[i].transform.rotation = Quaternion.Euler(fixedEuler);
        }

        for(int i = 0; i < 3; i++)
        {
            indicatorList[i].Ready(castingTime);

            yield return new WaitForSeconds(castingTime);

            RaycastHit hit;
            float radius = ((i + 1) / 3.0f) * BossAIManager.Instance.Pattern3Range / 2;
            List<Collider> colliderList = Physics.OverlapSphere(position, radius, LayerMask).ToList();

            GameObject playerObject = colliderList.Find(x => x.CompareTag("Player"))?.gameObject;

            if (playerObject)
            {
                Vector3 directionToTarget = (playerObject.transform.position - position).normalized;
                if(Vector3.Dot(forward, directionToTarget) > Mathf.Cos(BossAIManager.Instance.Pattern3Angle))
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
        BossAIManager.Instance.LastFinishedtimeList[3] = Time.time;
        EnemyRotation.IsFound = true;

    }

    public void SpecialAttack_04()
    {

    }

    public void OnSpecialAttack04End()
    {
        BossAIManager.Instance.LastFinishedtimeList[4] = Time.time;
    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(BossAIManager.Instance.DecideNextState());
    }

    private void ClusterInstantiate(Vector3 center, int count, float radiusX, float radiusY)
    {
        int placed = 0;

        while(placed < count)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Mathf.Sqrt(Random.Range(0, 1f));

            float x = Mathf.Cos(angle) * radius * radiusX;
            float y = Mathf.Sin(angle) * radius * radiusY;

            Vector3 spawnPoint = center + new Vector3(x, center.y, y);
            Lightning lightning = _lightningPool.Get();
            lightning.transform.position = spawnPoint;
            lightning.Init(BossAIManager.Instance.PatternCastingtimeList[1]);
            if (lightning.thisPool == null) lightning.thisPool = _lightningPool;

            placed++;
        }
    }
}
