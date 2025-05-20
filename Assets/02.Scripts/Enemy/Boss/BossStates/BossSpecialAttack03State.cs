using UnityEngine;

public class BossSpecialAttack03State : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
    }

    public void Update(AEnemy enemy)
    {
    }

    public void Exit(AEnemy enemy)
    {
    }
}
