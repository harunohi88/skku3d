using UnityEngine;

public class BossSpecialAttack01State : IState<AEnemy>
{
    private float _time = 0f;
    private bool _isStart = false;
    private EnemyPatternData _patternData;
    private bool _isAudioOn = false;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);

        _time = 0f;
        _patternData = BossAIManager.Instance.GetPatternData(1);
        _isAudioOn = false;

        enemy.SetAnimationTrigger("Run");
        enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);
    }

    public void Update(AEnemy enemy)
    {
        if(_isStart == false)
        {
            enemy.Agent.SetDestination(PlayerManager.Instance.Player.transform.position);

            if(enemy.Agent.remainingDistance < enemy.AttackDistance)
            {
                enemy.Agent.ResetPath();
                enemy.SetAnimationTrigger("SpecialAttack01");
                AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss1SpIdle);
                (enemy as ISpecialAttackable).SpecialAttack_01();
                _isStart = true;
            }
        }
        else
        {
            _time += Time.deltaTime;
            if (_isAudioOn == false && _time >= _patternData.CastingTime)
            {
                _isAudioOn = true;
                AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss1Sp1);
                CameraManager.Instance.CameraShake(0.3f, 0.4f);
            }

            if (_time >= _patternData.CastingTime + 0.5f)
            {
                (enemy as ISpecialAttackable).OnSpecialAttack01End();
                enemy.ChangeState(new BossIdleState());
            }
        }

    }

    public void Exit(AEnemy enemy)
    {
        enemy.Agent.speed = enemy.MoveSpeed;
    }
}
