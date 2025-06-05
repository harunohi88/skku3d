using UnityEngine;

public class Boss2IdleState : IState<AEnemy>
{
    // 보스 2가 플레이어를 인식하기 전 '대기 상태'
    private float _time = 0f;

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this); // 현재 State를 출력한다.
        _time = 0f;
        enemy.Agent.speed = 3.5f;
        enemy.SetAnimationTrigger("Idle");
        enemy.Agent.ResetPath();
        enemy.Agent.isStopped = true; // 보스의 추적 멈춤
        enemy.EnemyRotation.IsFound = false;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_time >= 1f)
        {
            if (Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position) < enemy.TraceDistance)
            {
                IState<AEnemy> state = Boss2AIManager.Instance.DecideNextState();
                enemy.ChangeState(state);
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.isStopped = false;
        enemy.EnemyRotation.IsFound = true;
    }
}

