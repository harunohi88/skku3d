using UnityEngine;

public class Boss2IdleState : IState<AEnemy>
{
    // 보스 2가 플레이어를 인식하기 전 '대기 상태'


    public void Enter(AEnemy enemy)
    {
        enemy.Agent.speed = 3.5f;
        enemy.SetAnimationTrigger("Idle");
        enemy.Agent.isStopped = true; // 보스의 추적 멈춤
        Debug.Log(this); // 현재 State를 출력한다.
    }
    public void Update(AEnemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position) < enemy.TraceDistance)
        {
            enemy.ChangeState(new Boss2TraceState());
            return;
        }
    }

    public void Exit(AEnemy enemy)
    {

    }
}

