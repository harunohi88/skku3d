using UnityEngine;

public class Boss2SpecialAttack02State : IState<AEnemy>
{
    // 애니메이션 진입으로 Idle 상태가 되면 손에 있던 무기가 비활성화되고, 복제된 무기가 활성화된다.
    // Idle -> 복제된 무기는 허공에 둥둥 떠있다.
    // Lift -> 복제된 무기는 위로 올라간다.
    // Throw -> 복제된 무기는 플레이어를 향해 날라간다.

    // 필요 속성

    // - 시간
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
        enemy.SetAnimationTrigger("SpecialAttack02_Idle");

        if (enemy is Boss_Ferex ferex)
        {
            Debug.Log("진짜 무기 꺼짐");
            ferex.WeaponOriginal.SetActive(false);
            ferex.WeaponCopied.SetActive(true);

            var weapon = ferex.WeaponCopied.GetComponent<WeaponMove>();
            weapon.SetState(WeaponMove.WeaponState.Idle);
        }

        _patternData = Boss2AIManager.Instance.GetPatternData(2, 0);

        if (_patternData != null)
        {
            _indicator = BossIndicatorManager.Instance.SetCircularIndicator(
                enemy.transform.position,
                _patternData.Radius,
                _patternData.Radius,
                0,
                _patternData.Angle,
                0.5f,
                _patternData.CastingTime,
                0,
                Color.red
                );
        }

    }
    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_currentPhase == 0 && _time >= _patternData.CastingTime)
        {
            _currentPhase = 1;
            _time = 0f;
            enemy.SetAnimationTrigger("SpecialAttack02_Lift");
            if (enemy is Boss_Ferex ferex)
            {
                var weapon = ferex.WeaponCopied.GetComponent<WeaponMove>();
                weapon.SetState(WeaponMove.WeaponState.Lift);
            }
        }
        else if (_currentPhase == 1 && _time >= 2f)
        {
            _currentPhase = 2;
            _time = 0f;

            if (enemy is Boss_Ferex ferex)
            {
                var weapon = ferex.WeaponCopied.GetComponent<WeaponMove>();
                weapon.Center = ferex.transform;
                weapon.SetState(WeaponMove.WeaponState.Throw);
            }
        }
        else if (_currentPhase == 2 && _time >= 1f)
        {
            _currentPhase = 3;
        }
    }

    public void Exit(AEnemy enemy)
    {
        if (_indicator != null) GameObject.Destroy(_indicator.gameObject);
        if (enemy is Boss_Ferex ferex)
        {
            Debug.Log("진짜 무기 켜짐");
            ferex.WeaponCopied.SetActive(false);
            ferex.WeaponOriginal.SetActive(true);
        }
        enemy.Agent.isStopped = false;
    }
}
