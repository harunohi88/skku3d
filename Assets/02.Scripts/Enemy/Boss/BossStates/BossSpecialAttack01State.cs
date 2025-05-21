using UnityEngine;

public class BossSpecialAttack01State : IState<AEnemy>
{
    private float _time = 0f;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack01");
        (enemy as Boss_MechanicGolem).SpecialAttack_01();
        
        _time = 0f;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;

        if (_time >= BossAIManager.Instance.PatternCastingtimeList[1] + 0.5f)
        {
            // 임시
            (enemy as Boss_MechanicGolem).OnSpecialAttack01End();
            enemy.ChangeState(BossAIManager.Instance.DecideNextState());
        }
    }

    public void Exit(AEnemy enemy)
    {
    }
}
