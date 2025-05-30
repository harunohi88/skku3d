using UnityEngine;

public class Boss3DieState : IState<AEnemy>
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