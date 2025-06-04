using Unity.VisualScripting;
using UnityEngine;

public class Boss3TraceState : IState<AEnemy>
{
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
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
        
        _time += Time.deltaTime;
        if (_time >= 2f)
        {
            IState<AEnemy> state = Boss3AIManager.Instance.DecideNextState();
            if (state is Boss3TraceState) return;
            else enemy.ChangeState(state);
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.ResetPath();
    }
}
