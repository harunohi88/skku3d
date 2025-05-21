using UnityEngine;

public class BossSpecialAttack03State : IState<AEnemy>
{
    private float _time = 0f;
    private int _currentOrder = 0;

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack03_1");
        (enemy as Boss_MechanicGolem).SpecialAttack_01();

        _time = 0f;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if(_currentOrder == 0)
        {
            if (_time >= BossAIManager.Instance.PatternCastingtimeList[1])
            {
                enemy.SetAnimationTrigger("SpecialAttack03_2_Idle");
                enemy.EnemyRotation.IsFound = true;
                _currentOrder++;
                _time = 0;
            }
        }
        else if(_currentOrder == 1)
        {
            if(_time >= BossAIManager.Instance.PatternCastingtimeList[3] / 3)
            {
                enemy.SetAnimationTrigger("SpecialAttack03_2");
                enemy.EnemyRotation.IsFound = false;
                _currentOrder++;
                _time = 0;

            }
        }
        else if(_currentOrder == 2)
        {
            if(_time >= BossAIManager.Instance.PatternCastingtimeList[3] + 1f)
            {
                enemy.ChangeState(BossAIManager.Instance.DecideNextState());
            }
        }
        
    }

    public void Exit(AEnemy enemy)
    {
    }
}
