using UnityEngine;

public class BossSpecialAttack04State : IState<AEnemy>
{
    private EnemyPatternData _patternData;
    private SkillIndicator _indicator;
    private int _currentOrder = 0;
    private float _time;
    private float _tickTime;
    private float _enemyOriginStoppingDistance;

    public Vector2 MapCenter = new Vector2(0, 0);

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack04_Idle");
        if (_patternData == null) _patternData = BossAIManager.Instance.GetPatternData(4);
        _indicator = BossIndicatorManager.Instance.SetCircularIndicator(enemy.transform.position, _patternData.Range, _patternData.Range, 0, _patternData.Angle, _patternData.InnerRange, _patternData.CastingTime, 0);
        _enemyOriginStoppingDistance = enemy.Agent.stoppingDistance;
        enemy.Agent.speed = enemy.MoveSpeed - 0.5f;
    }

    public void Update(AEnemy enemy)
    {
        if (_indicator != null) _indicator.transform.position = enemy.transform.position;

        _time += Time.deltaTime;
        if (_currentOrder == 0)
        {
            if (_time >= _patternData.CastingTime)
            {
                enemy.SetAnimationTrigger("SpecialAttack04");
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


            _tickTime += Time.deltaTime;
            if (_tickTime >= 0.2f)
            {
                float distanceToPlayer = Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position);

                if (distanceToPlayer >= _patternData.Radius * _patternData.InnerRange && distanceToPlayer <= _patternData.Radius)
                {
                    Debug.Log("Pattern 4 데미지 발생");
                }
                _tickTime = 0f;
            }

            if (_time >= _patternData.Duration)
            {
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
