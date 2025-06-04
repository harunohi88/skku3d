using UnityEngine;

public class BossSpecialAttack03State : IState<AEnemy>
{
    private float _time = 0f;
    private int _currentOrder = 0;
    private EnemyPatternData _patternData;
    private bool _isStart = false;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        _time = 0f;
        _currentOrder = 0;

        _patternData = BossAIManager.Instance.GetPatternData(3, 0);


        enemy.SetAnimationTrigger("Run");
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
    }

    public void Update(AEnemy enemy)
    {
        if (_isStart == false)
        {
            enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);

            if (enemy.Agent.remainingDistance < enemy.AttackDistance)
            {
                enemy.Agent.ResetPath();
                enemy.SetAnimationTrigger("SpecialAttack03_1");
                AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss1Sp3_1);
                (enemy as ISpecialAttackable).SpecialAttack_01();
                _isStart = true;
            }
        }
        else
        {

            _time += Time.deltaTime;
            if (_patternData == null) return;

            if (_currentOrder == 0)
            {
                if (_time >= _patternData.CastingTime)
                {
                    CameraManager.Instance.CameraShake(0.3f, 0.4f);
                    _currentOrder++;
                    _time = 0f;
                    _patternData = BossAIManager.Instance.GetPatternData(3, 1);
                }
            }
            else if (_currentOrder == 1)
            {
                enemy.SetAnimationTrigger("SpecialAttack03_2");
                enemy.EnemyRotation.IsFound = false;
                _currentOrder++;
                _time = 0f;
            }
            else if (_currentOrder == 2)
            {
                if (_time >= _patternData.CastingTime + 2.5f)
                {
                    enemy.EnemyRotation.IsFound = true;
                    enemy.ChangeState(new BossIdleState());
                }
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
    }
}
