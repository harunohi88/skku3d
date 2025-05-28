using UnityEngine;

public class Knife_DynamicRune : ADynamicRuneObject
{
    private bool _isStabbing;
    public int StabCount = 3;
    public override void Init(Damage damage, float radius, float approachDuration, Vector3 startPosition, Transform targetTransform)
    {
        base.Init(damage, radius, approachDuration, startPosition, targetTransform);
        _isStabbing = false;
    }

    public override void Update()
    {
        if(_isStabbing == false)
        {
            _time += Time.deltaTime / _approachDuration;
            transform.position = GetQuadraticBezierPoint(_time, _startPosition, _controlPoint, _targetTransform.position);

            if(_time >= 1f)
            {
                _isStabbing = true;
                _time = 0f;
            }
        }
        else
        {
            _time += Time.deltaTime * StabCount;
            float angle = _time * Mathf.PI * 2;
            float x = Mathf.Sin(angle);
            float y = Mathf.Sin(angle) * Mathf.Cos(angle);
            Vector3 offset = new Vector3(x * 2f, y * 2f, 0);
            transform.position = _targetTransform.position + offset;
        }
    }
}
