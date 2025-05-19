using UnityEngine;

public class AttackState : IState<AEnemy>
{
    private float _time;
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("MoveToAttackDelay");
        _time = 0f;
    }

    public void Update(AEnemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, Test_LSJ.PlayerManager.Instance.Player.transform.position);

        if (distanceToPlayer >= enemy.AttackDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }

        _time += Time.deltaTime;
        if(_time >= enemy.AttackCooltime)
        {
            enemy.SetAnimationTrigger("AttackDelayToAttack");
            Debug.Log("공격!");
            _time = 0f;
        }
    }

    public void Exit(AEnemy enemy)
    {


    }
}
