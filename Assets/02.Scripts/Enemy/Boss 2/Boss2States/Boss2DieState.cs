public class Boss2DieState : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("Die");
    }
    public void Update(AEnemy entity)
    {

    }

    public void Exit(AEnemy entity)
    {

    }
}
