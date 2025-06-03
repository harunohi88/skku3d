using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BossAIManager))]
public class Boss_MechanicGolem : AEnemy, ISpecialAttackable
{
    public Collider WeaponCollider;
    public Lightning LightningPrefab;
    private ObjectPool<Lightning> _lightningPool;
    private int _baseAttackCount = 0;
    private int _attackCount = 0;
    public LaserAttack LaserAttack;
    public LaserScript Laser;

    protected void Start()
    {
        _lightningPool = new ObjectPool<Lightning>(LightningPrefab, 20, GameObject.FindGameObjectWithTag("Pool").transform);

        Init(null);
    }

    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new BossIdleState());
        BossUIManager.Instance.SetBossUI("Lumen", MaxHealth); ///// HealthBar 추가한 코드
    }

    public override void TakeDamage(Damage damage)
    {
        if (_stateMachine.CurrentState is BossDieState) return;
        Health -= damage.Value;
        BossUIManager.Instance.UPdateHealth(Health); ///// HealthBar 추가한 코드

        EnemyFloatingTextManager.Instance.TriggerFeedback(damage.Value, transform.position + Vector3.up * 2f, damage.IsCritical);

        EnemyHitEffect.PlayHitEffect(DamagedTime);

        if (Health <= 0)
        {
            ChangeState(new BossDieState());
            return;
        }
    }

    public override void Attack()
    {
        float disTanceToPlayer = Vector3.Distance(transform.position, PlayerManager.Instance.Player.transform.position);
        Vector3 directionToPlayer = (PlayerManager.Instance.Player.transform.position - transform.position).normalized;
        Vector3 targetPosition = PlayerManager.Instance.Player.transform.position - directionToPlayer * (AttackDistance / 2f);

        if (disTanceToPlayer <= AttackDistance / 2f) targetPosition = transform.position;

        Agent.enabled = false;
        transform.DOMove(targetPosition, 0.1f).SetEase(Ease.InQuad);

        BossEffectManager.Instance.PlayBoss1Particle(_baseAttackCount);
        WeaponCollider.enabled = true;
        EnemyRotation.IsFound = true;
        _attackCount = 0;
    }

    public void OnBaseAttackEnd()
    {
        WeaponCollider.enabled = false;
        _baseAttackCount++;
        if (_baseAttackCount >= 2)
        {
            _baseAttackCount = 0;
            Agent.enabled = true;
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
            BossEffectManager.Instance.PlayBoss1Particle(2);

            EnemyPatternData _patternData = BossAIManager.Instance.GetPatternData(2, 0);
            Damage damage = new Damage();
            damage.Value = _patternData.Damage;
            damage.From = this.gameObject;

            Collider[] colliders = Physics.OverlapSphere(transform.position, _patternData.Radius, LayerMask.GetMask("Player"));
            if(colliders.Length > 0)
            {
                PlayerManager.Instance.Player.TakeDamage(damage);
            }
            
            EnemyRotation.IsFound = false;
        }
        else if (_attackCount == 1)
        {
            EnemyPatternData _patternData = BossAIManager.Instance.GetPatternData(2, 1);
            Damage damage = new Damage();
            damage.Value = _patternData.Damage;
            damage.From = this.gameObject;

            LaserAttack.Init(_patternData.Duration - 0.3f);

            Laser.ShootLaser(_patternData.Duration - 0.3f);

            LaserAttack.Damage = damage;
            LaserAttack.GetComponent<Collider>().enabled = true;

            LaserAttack.gameObject.SetActive(true);

            //List<Collider> colliderList = Physics.OverlapSphere(transform.position, _patternData.Range, LayerMask).ToList();

            //GameObject playerObject = colliderList.Find(x => x.CompareTag("Player"))?.gameObject;

            //if (playerObject)
            //{
            //    Vector3 directionToTarget = playerObject.transform.position - transform.position;
            //    if (Vector3.Dot(transform.forward, directionToTarget.normalized) > 0 && Mathf.Abs(Vector3.Dot(transform.right, directionToTarget)) <= _patternData.Width / 2)
            //    {
            //        Debug.Log("Pattern 3 데미지 발생");
            //    }
            //}
        }
    }

    public void OnSpecialAttack02End()
    {
        _attackCount++;
        if (_attackCount >= 2)
        {
            WeaponCollider.enabled = false;
            BossAIManager.Instance.SetLastFinishedTime(2, Time.time);
            OnAnimationEnd();
        }
        else
        {
            EnemyRotation.IsFound = true;
            WeaponCollider.enabled = false;
            LaserAttack.gameObject.SetActive(false);
        }
    }

    public void SpecialAttack_03()
    {
        Vector3 position = transform.position;
        StartCoroutine(SpecialAttack03_Coroutine(position, transform.forward));
    }

    public IEnumerator SpecialAttack03_Coroutine(Vector3 position, Vector3 forward)
    {
        var patternData = BossAIManager.Instance.GetPatternData(3, 1);
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
                Color.red,
                false
            ));

            Quaternion rotation = Quaternion.LookRotation(forward);
            Vector3 fixedEuler = new Vector3(90f, 0f, -rotation.eulerAngles.y);
            indicatorList[i].transform.rotation = Quaternion.Euler(fixedEuler);
        }

        for (int i = 0; i < 3; i++)
        {
            indicatorList[i].Ready(castingTime);
            yield return new WaitForSeconds(castingTime);

            BossEffectManager.Instance.PlayBoss1Particle(i + 4);
            float radius = ((i + 1) / 3.0f) * patternData.Range / 2;
            List<Collider> colliderList = Physics.OverlapSphere(position, radius, LayerMask).ToList();

            GameObject playerObject = colliderList.Find(x => x.CompareTag("Player"))?.gameObject;

            if (playerObject)
            {
                Vector3 directionToTarget = (playerObject.transform.position - position).normalized;
                if (Vector3.Dot(forward, directionToTarget) > Mathf.Cos(patternData.Angle))
                {
                    float distance = Vector3.Distance(playerObject.transform.position, position);
                    if (distance > (i / (float)(i + 1)) * radius)
                    {
                        Damage damage = new Damage();
                        damage.Value = patternData.Damage;
                        damage.From = gameObject;
                        PlayerManager.Instance.Player.TakeDamage(damage);
                    }
                }
            }
        }
    }

    public void OnSpecialAttack03End()
    {
        BossAIManager.Instance.SetLastFinishedTime(3, Time.time);
    }

    public void SpecialAttack_04()
    {
    }

    public void OnSpecialAttack04End()
    {
        BossAIManager.Instance.SetLastFinishedTime(4, Time.time);
    }

    public override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        ChangeState(new BossIdleState());
    }

    private void ClusterInstantiate(Vector3 center, int count, float radiusX, float radiusY)
    {
        int placed = 0;
        var patternData = BossAIManager.Instance.GetPatternData(1);

        while (placed < count)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            float radius = Mathf.Sqrt(Random.Range(0, 1f));

            float x = Mathf.Cos(angle) * radius * radiusX;
            float y = Mathf.Sin(angle) * radius * radiusY;

            Vector3 spawnPoint = center + new Vector3(x, center.y, y);
            Lightning lightning = _lightningPool.Get();
            lightning.transform.position = spawnPoint;
            lightning.Init(patternData.CastingTime, patternData.Radius, patternData.Duration);
            if (lightning.thisPool == null) lightning.thisPool = _lightningPool;

            placed++;
        }
    }
}
