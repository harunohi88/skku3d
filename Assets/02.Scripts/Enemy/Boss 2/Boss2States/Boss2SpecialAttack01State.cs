public class Boss2SpecialAttack01State : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("SpecialAttack01");
    }
    public void Update(AEnemy entity)
    {

    }

    public void Exit(AEnemy entity)
    {

    }
}
