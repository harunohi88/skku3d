using UnityEngine;

public class BossSpecialAttack04State : IState<AEnemy>
{
    private EnemyPatternData _patternData;
    private SkillIndicator _indicator;
    private int _currentOrder = 0;
    private float _time;
    private float _tickTime;

    private float _soundTimer = 0f;
    private float _soundTick = 0.4f;
    private float _enemyOriginStoppingDistance;

    public Vector2 MapCenter = new Vector2(0, 0);

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack04_Idle");
        if (_patternData == null) _patternData = BossAIManager.Instance.GetPatternData(4);
        _indicator = BossIndicatorManager.Instance.SetCircularIndicator(enemy.transform.position, _patternData.Radius * 2, _patternData.Radius * 2, 0, _patternData.Angle, _patternData.InnerRange, _patternData.CastingTime, 0, Color.red);
        _enemyOriginStoppingDistance = enemy.Agent.stoppingDistance;
        enemy.Agent.speed = 10f;

        _soundTimer = _soundTick;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_currentOrder == 0)
        {
            if (_time >= _patternData.CastingTime)
            {
                AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss1Sp4_2, true);
                enemy.SetAnimationTrigger("SpecialAttack04");
                BossEffectManager.Instance.PlayBoss1Particle(3);
                _currentOrder++;
                _time = 0;

                enemy.Agent.SetDestination(GetRandomPosition(enemy));
                enemy.Agent.stoppingDistance = 0.01f;
            }
        }
        else if (_currentOrder == 1)
        {
            if (enemy.Agent.remainingDistance - 0.1f <= enemy.Agent.stoppingDistance)
            {
                enemy.Agent.SetDestination(GetRandomPosition(enemy));
            }

            _soundTimer += Time.deltaTime;
            if(_soundTimer >= _soundTick)
            {
                _soundTimer = 0f;
                AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss1Sp4_1);
            }


            _tickTime += Time.deltaTime;
            if (_tickTime >= 0.2f)
            {
                float distanceToPlayer = Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position);

                if (distanceToPlayer >= _patternData.Radius * _patternData.InnerRange && distanceToPlayer <= _patternData.Radius)
                {
                    Damage damage = new Damage();
                    damage.Value = _patternData.Damage;
                    damage.From = enemy.gameObject;
                    PlayerManager.Instance.Player.TakeDamage(damage);
                }
                _tickTime = 0f;
            }

            if (_time >= _patternData.Duration)
            {
                BossEffectManager.Instance.StopBoss1Particle(3);
                AudioManager.Instance.StopEnemyAudio(EnemyAudioType.Boss1Sp4_2);

                enemy.ChangeState(new BossTraceState());
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.stoppingDistance = _enemyOriginStoppingDistance;
        enemy.Agent.ResetPath();
        enemy.Agent.speed = enemy.MoveSpeed;
    }

    public Vector3 GetRandomPosition(AEnemy enemy)
    {
        Vector2 randomPosition = Random.insideUnitCircle;
        return enemy.transform.position + new Vector3(randomPosition.x * _patternData.Radius, enemy.transform.position.y, randomPosition.y * _patternData.Radius);
    }
}
