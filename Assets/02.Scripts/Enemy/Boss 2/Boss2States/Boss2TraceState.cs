using UnityEngine;

public class Boss2TraceState : IState<AEnemy>
{
    private float _time = 0f;

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        _time = 0f;
        enemy.SetAnimationTrigger("Trace");
        //AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss2Trace);
        enemy.EnemyRotation.IsFound = true;
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
    }
    public void Update(AEnemy enemy)
    {
        Debug.Log("보스2 Trace 업데이트 진입");
        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            Debug.Log($"RemaningDistance: {enemy.Agent.remainingDistance}, stoppingDistance: {enemy.Agent.stoppingDistance}");
            enemy.ChangeState(new Boss2IdleState());
        }


        _time += Time.deltaTime;
        if (_time >= 3f)
        {
            Debug.Log("보스2 Trace 업데이트 진입 3초 지남");
            IState<AEnemy> state = Boss2AIManager.Instance.DecideNextState();
            if (state is Boss2TraceState) return;
            else enemy.ChangeState(state);
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.ResetPath();
    }
}
