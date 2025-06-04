using UnityEngine;

public class Enemy_Melee : AEnemy
{
    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new IdleState());
    }

    public override void Attack()
    {
        EnemyRotation.IsFound = false;
        Vector3 directionToPlayer = PlayerManager.Instance.Player.transform.position - transform.position;
        directionToPlayer = directionToPlayer.normalized;
        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.Player.transform.position);
        if(distance <= AttackDistance)
        {
            if(Vector3.Dot(transform.forward, directionToPlayer) > 0)
            {
                Damage damage = new Damage();
                damage.Value = Damage;
                damage.From = gameObject;
                PlayerManager.Instance.Player.TakeDamage(damage);
            }
        }
    }

    public override void OnAnimationEnd()
    {
        EnemyRotation.IsFound = true;
        ChangeState(new TraceState());
    }
}
