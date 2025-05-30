using UnityEngine;

public class Boss2BaseAttackState : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);

        //enemy.Agent.enabled = false;
        enemy.SetAnimationTrigger("BaseAttack");
        enemy.EnemyRotation.IsFound = false;
        enemy.Agent.isStopped = true;
        enemy.Agent.ResetPath();

        // 회전 고정 해제
        // 쿨타임 체크 및 초기화 준비
    }
    public void Update(AEnemy enemy)
    {
        //if (!enemy.IsPlayingAnimation("BaseAttack"))
        //{
        //    Debug.Log("BaseAttack 애니메이션 끝 → 상태 전환");
        //    enemy.ChangeState(new Boss2TraceState());
        //}
    }

    public void Exit(AEnemy enemy)
    {
        // 무기 콜라이더 비활성화
        // 상태 초기화
        //if (!enemy.Agent.enabled)
        //{
        //    enemy.Agent.enabled = true;
        //    enemy.Agent.ResetPath();
        //}
        //enemy.Agent.isStopped = false;
    }
}
