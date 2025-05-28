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
        // 스킬 이펙트

        EnemyRotation.IsFound = false;
        Vector3 directionToPlayer = PlayerManager.Instance.Player.transform.position - transform.position;
        directionToPlayer = directionToPlayer.normalized;
        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.Player.transform.position);
        if(distance <= AttackDistance)
        {
            if(Vector3.Dot(transform.forward, directionToPlayer) > 0)
            {
                Debug.Log("데미지를 줍니다");
            }
        }
    }
}
