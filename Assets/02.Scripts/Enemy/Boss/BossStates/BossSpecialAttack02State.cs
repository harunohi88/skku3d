using UnityEngine;

public class BossSpecialAttack02State : IState<AEnemy>
{
    private float _time = 0f;
    private int _currentOrder = 0;
    private SkillIndicator _indicator;
    private EnemyPatternData _patternData;

    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);
        enemy.SetAnimationTrigger("SpecialAttack02_1_Idle");
        _time = 0f;
        
        _patternData = BossAIManager.Instance.GetPatternData(2, 0);
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
                0
            );
        }
    }

    public void Update(AEnemy enemy)
    {
        if (_indicator) _indicator.SetPosition(enemy.transform.position);

        _time += Time.deltaTime;
        if (_patternData == null) return;

        if(_currentOrder == 0)
        {
            if (_time >= _patternData.CastingTime)
            {
                _time = 0f;
                enemy.SetAnimationTrigger("SpecialAttack02_1");
                _currentOrder++;


            }
        }
        else if(_currentOrder == 1)
        {
            if (_time >= 2f)
            {
                _currentOrder++;
                enemy.EnemyRotation.IsFound = true;

                _patternData = BossAIManager.Instance.GetPatternData(2, 1);
                _indicator = BossIndicatorManager.Instance.SetSquareIndicator(enemy.transform.position, _patternData.Width, _patternData.Range, 0, 0, _patternData.CastingTime, 0);
                _time = 0f;
            }
        }
        else if(_currentOrder == 2)
        {
            Quaternion rotation = Quaternion.LookRotation(enemy.transform.forward);
            Vector3 fixedEuler = new Vector3(90f, 0f, -rotation.eulerAngles.y);
            _indicator.transform.rotation = Quaternion.Euler(fixedEuler);

            if (_time >= _patternData.CastingTime)
            {
                _time = 0f;
                enemy.SetAnimationTrigger("SpecialAttack02_2");
                _currentOrder++;
                enemy.EnemyRotation.IsFound = false;
            }
        }
    }

    public void Exit(AEnemy enemy)
    {
    }
}
