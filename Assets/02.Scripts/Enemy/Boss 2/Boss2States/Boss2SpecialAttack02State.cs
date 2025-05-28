public class Boss2SpecialAttack02State : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("SpecialAttack02");
    }
    public void Update(AEnemy entity)
    {

    }

    public void Exit(AEnemy entity)
    {

    }
}
