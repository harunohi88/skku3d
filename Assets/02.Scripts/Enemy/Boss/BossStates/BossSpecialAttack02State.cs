using UnityEngine;

public class BossSpecialAttack02State : IState<AEnemy>
{
    private float _time = 0f;
    private int _currentOrder = 0;
    private SkillIndicator _indicator;
    private EnemyPatternData _patternData;
    private bool _isStart = false;

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        _time = 0f;


        enemy.SetAnimationTrigger("Run");
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
    }

    public void Update(AEnemy enemy)
    {
        if (_isStart == false)
        {
            enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);

            if (enemy.Agent.remainingDistance < enemy.AttackDistance)
            {
                enemy.Agent.ResetPath();
                enemy.SetAnimationTrigger("SpecialAttack02_1_Idle");
                _patternData = BossAIManager.Instance.GetPatternData(2, 0);
                if (_patternData != null)
                {
                    _indicator = BossIndicatorManager.Instance.SetCircularIndicator(
                        enemy.transform.position,
                        _patternData.Radius * 2,
                        _patternData.Radius * 2,
                        0,
                        _patternData.Angle,
                        _patternData.InnerRange,
                        _patternData.CastingTime,
                        0,
                        Color.red
                    );
                }
                _isStart = true;
            }
        }
        else
        {
            if (_indicator) _indicator.SetPosition(enemy.transform.position);

            _time += Time.deltaTime;
            if (_patternData == null) return;

            if (_currentOrder == 0)
            {
                if (_time >= _patternData.CastingTime)
                {
                    _time = 0f;
                    enemy.SetAnimationTrigger("SpecialAttack02_1");
                    _currentOrder++;
                }
            }
            else if (_currentOrder == 1)
            {
                if (_time >= 2f)
                {
                    _currentOrder++;
                    enemy.EnemyRotation.IsFound = true;

                    _patternData = BossAIManager.Instance.GetPatternData(2, 1);
                    _indicator = BossIndicatorManager.Instance.SetSquareIndicator(enemy.transform.position, _patternData.Width, _patternData.Range, 0, 0, _patternData.CastingTime, 0, Color.red);
                    _time = 0f;
                }
            }
            else if (_currentOrder == 2)
            {
                Quaternion rotation = Quaternion.LookRotation(enemy.transform.forward);
                Vector3 fixedEuler = new Vector3(90f, 0f, -rotation.eulerAngles.y);
                _indicator.transform.rotation = Quaternion.Euler(fixedEuler);

                if (_time >= _patternData.CastingTime)
                {
                    _time = 0f;
                    enemy.EnemyRotation.IsFound = false;
                    (enemy as ISpecialAttackable).SpecialAttack_02();
                    _currentOrder++;
                }
            }
            else
            {
                if (_time >= _patternData.Duration)
                {
                    (enemy as ISpecialAttackable).OnSpecialAttack02End();
                    enemy.ChangeState(new BossIdleState());
                }
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
    }
}
