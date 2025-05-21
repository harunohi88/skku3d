using UnityEngine;

public class BossSpecialAttack02State : IState<AEnemy>
{
    private float _time;
    private int _currentOrder = 0;
    private SkillIndicator indicator;

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack02_1_Idle");
        _time = 0;
        indicator = BossIndicatorManager.Instance.SetIndicator(enemy.transform.position, BossAIManager.Instance.Pattern2Radius, BossAIManager.Instance.Pattern2Radius, 0, 360, 0, BossAIManager.Instance.Patter2FirstCastingtime, 0);
    }

    public void Update(AEnemy enemy)
    {
        if (indicator) indicator.transform.position = enemy.transform.position;

        _time += Time.deltaTime;
        if(_currentOrder == 0)
        {
            if (_time >= BossAIManager.Instance.Patter2FirstCastingtime)
            {
                _time = 0;
                enemy.SetAnimationTrigger("SpecialAttack02_1");
                _currentOrder++;
            }
        }
        else if(_currentOrder == 1)
        {
            _time = 0;
            if (enemy.IsPlayingAnimation("SpecialAttack02_1") == false)
            {
                _currentOrder++;
                enemy.EnemyRotation.IsFound = true;
            }
        }
        else if(_currentOrder == 2)
        {
            if(_time >= BossAIManager.Instance.PatternCooltimeList[2])
            {
                _time = 0;
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
