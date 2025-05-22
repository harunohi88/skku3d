using Unity.VisualScripting;
using UnityEngine;

public class BossTraceState : IState<AEnemy>
{
    // 최소 1초는 기다림
    private float _time = 0f;
    private bool _isIdle = false;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("Run");
        enemy.EnemyRotation.IsFound = true;
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
    }

    public void Update(AEnemy enemy)
    {
        //float distanceToPlayer = Vector3.Distance(enemy.transform.position, PlayerManager.Instance.Player.transform.position);
        //if (distanceToPlayer < enemy.Agent.stoppingDistance + 1f)
        //{
        //    if (_isIdle == false)
        //    {
        //        enemy.SetAnimationTrigger("Idle");
        //        _isIdle = true;
        //    }
        //    enemy.Agent.ResetPath();
        //}
        //else
        //{
        //    if (_isIdle)
        //    {
        //        enemy.SetAnimationTrigger("Run");
        //        _isIdle = false;
        //    }
            enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
        //}

        _time += Time.deltaTime;
        if(_time >= 3f)
        {
            IState<AEnemy> state = BossAIManager.Instance.DecideNextState();
            if (state is BossTraceState) return;
            else enemy.ChangeState(state);
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.ResetPath();
    }
}
