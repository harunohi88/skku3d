using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BossAIManager))]
public class Boss_MechanicGolem : AEnemy
{
    public Collider WeaponCollider;
    public List<Lightning> LightningPrefabList;
    private ObjectPool<Lightning> _lightningPool;
    private int _baseAttackCount = 0;

    protected override void Start()
    {
        base.Start();
        _lightningPool = new ObjectPool<Lightning>(LightningPrefabList, 20);
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
        _baseAttackCount++;
        if(_baseAttackCount >= 4)
        {
            _baseAttackCount = 0;
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

    }

    public void OnSpecialAttack02End()
    {
        BossAIManager.Instance.LastFinishedtimeList[2] = Time.time;
    }

    public void SpecialAttack_03()
    {

    }

    public void OnSpecialAttack03End()
    {
        BossAIManager.Instance.LastFinishedtimeList[3] = Time.time;
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
