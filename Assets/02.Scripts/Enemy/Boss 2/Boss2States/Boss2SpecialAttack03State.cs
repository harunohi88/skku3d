using UnityEngine;

public class Boss2SpecialAttack03State : IState<AEnemy>
{
    private float _time = 0f;
    private int _currentPhase = 0;
    private SkillIndicator _indicator;
    private EnemyPatternData _patternData;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        _time = 0f;
        _currentPhase = 0;

        enemy.Agent.ResetPath();
        enemy.Agent.isStopped = true;
        enemy.EnemyRotation.IsFound = false;
        enemy.SetAnimationTrigger("SpecialAttack03_Idle");
        AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss2Sp3Idle);

        _patternData = Boss2AIManager.Instance.GetPatternData(3, 0);

        if (_patternData != null)
        {
            _indicator = BossIndicatorManager.Instance.SetCircularIndicator(
                enemy.transform.position,
                _patternData.Radius,
                _patternData.Radius,
                0,
                _patternData.Angle,
                0,
                _patternData.CastingTime,
                0,
                Color.green
                );
            (enemy as Boss_Ferex)?.SetIndicatorPosition(_indicator.transform.position);
        }
        Quaternion _indicatorPos = Quaternion.Euler(90, 0, -enemy.transform.eulerAngles.y);
        _indicator.transform.rotation = _indicatorPos;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_currentPhase == 0 && _time >= _patternData.CastingTime)
        {
            _currentPhase = 1;
            _time = 0;
            enemy.SetAnimationTrigger("SpecialAttack03_Roaring");
            CameraManager.Instance.CameraShake(0.5f, 2f);
        }
    }

    public void Exit(AEnemy enemy)
    {
        if (_indicator != null) GameObject.Destroy(_indicator.gameObject);
        enemy.Agent.isStopped = false;
    }
}
