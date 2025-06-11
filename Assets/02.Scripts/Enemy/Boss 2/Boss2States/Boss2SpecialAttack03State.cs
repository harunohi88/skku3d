using UnityEngine;

public class Boss2SpecialAttack03State : IState<AEnemy>
{
    private float _time = 0f;
    private int _currentPhase = 0;
    private SkillIndicator _indicator;
    private SkillIndicator _indicator2;
    private EnemyPatternData _patternData;
    private EnemyPatternData _patternData2;
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
        _patternData2 = Boss2AIManager.Instance.GetPatternData(3, 1);

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

        if (_patternData2 != null)
        {
            _indicator2 = BossIndicatorManager.Instance.SetCircularIndicator(
                enemy.transform.position,
                _patternData2.Radius,
                _patternData2.Radius,
                0,
                _patternData2.Angle,
                _patternData2.InnerRange,
                _patternData2.CastingTime,
                0,
                new Color(1.0f, 0.5f, 0.0f)
            );
            Quaternion _indicatorPos2 = Quaternion.Euler(90, 0, -enemy.transform.eulerAngles.y);
            _indicator2.transform.rotation = _indicatorPos2;
        }
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
        if(_indicator2 != null) GameObject.Destroy(_indicator2.gameObject);
        enemy.Agent.isStopped = false;
    }
}