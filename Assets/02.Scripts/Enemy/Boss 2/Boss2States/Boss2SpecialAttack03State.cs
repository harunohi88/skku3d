using UnityEngine;

public class Boss2SpecialAttack03State : IState<AEnemy>
{
    private float _time = 0;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.Agent.ResetPath();
        enemy.SetAnimationTrigger("SpecialAttack03");
        enemy.Agent.speed = enemy.MoveSpeed + 1;
        enemy.EnemyRotation.IsFound = false;
        enemy.Agent.isStopped = true;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_time >= 4f)
        {
            enemy.SetAnimationTrigger("SpecialAttack03_Idle");
        }
        //if (!enemy.IsPlayingAnimation("SpecialAttack03"))
        //{
        //    Debug.Log("SpecialAttack03 애니메이션 끝 → 상태 전환");
        //    enemy.ChangeState(new Boss2TraceState());
        //}
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.isStopped = false;
        enemy.Agent.speed = enemy.MoveSpeed;
    }
}
