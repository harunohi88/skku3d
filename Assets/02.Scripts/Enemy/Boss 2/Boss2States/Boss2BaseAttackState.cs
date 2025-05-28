public class Boss2BaseAttackState : IState<AEnemy>
{
    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("BaseAttack");
        // 회전 고정 해제
        // 쿨타임 체크 및 초기화 준비
    }
    public void Update(AEnemy entity)
    {
        // 일정 시간 후 상태 전환 체크
        // 다음 상태 결정
    }

    public void Exit(AEnemy entity)
    {
        // 무기 콜라이더 비활성화
        // 상태 초기화
    }
}
