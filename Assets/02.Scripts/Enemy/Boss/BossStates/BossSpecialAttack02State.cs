using UnityEngine;

public class BossSpecialAttack02State : IState<AEnemy>
{
    private float _time = 0f;
    private int _currentOrder = 0;
    private SkillIndicator _indicator;
    private EnemyPatternData _patternData;

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack02_1_Idle");
        _time = 0f;
        
        _patternData = BossAIManager.Instance.GetPatternData(2, 0);
        if (_patternData != null)
        {
            _indicator = BossIndicatorManager.Instance.SetCircularIndicator(
                enemy.transform.position, 
                _patternData.Radius, 
                _patternData.Radius, 
                0, 
                360, 
                0, 
                _patternData.CastingTime, 
                0
            );
        }
    }

    public void Update(AEnemy enemy)
    {
        if (_indicator) _indicator.transform.position = enemy.transform.position;

        _time += Time.deltaTime;
        var patternData = BossAIManager.Instance.GetPatternData(2);
        if (patternData == null) return;

        if(_currentOrder == 0)
        {
            if (_time >= patternData.CastingTime)
            {
                _time = 0f;
                enemy.SetAnimationTrigger("SpecialAttack02_1");
                _currentOrder++;
            }
        }
        else if(_currentOrder == 1)
        {
            if (enemy.IsPlayingAnimation("SpecialAttack02_1") == false)
            {
                _currentOrder++;
                enemy.EnemyRotation.IsFound = true;
                _patternData = BossAIManager.Instance.GetPatternData(2, 1);
            }
        }
        else if(_currentOrder == 2)
        {
            if(_time >= patternData.CoolTime)
            {
                _time = 0f;
                enemy.SetAnimationTrigger("SpecialAttack02_2");
                _currentOrder++;
                enemy.EnemyRotation.IsFound = false;
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
    }
}
