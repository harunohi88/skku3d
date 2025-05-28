public class Boss2SpecialAttack03State : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("SpecialAttack03");
    }

    public void Exit(AEnemy enemy)
    {

    }

    public void Update(AEnemy enemy)
    {

    }
}
