using Unity.VisualScripting;
using UnityEngine;

public abstract class ADynamicRuneObject : MonoBehaviour
{
    protected Damage _damage;
    protected float _radius;
    protected Vector3 _startPosition;
    protected Transform _targetTransform;
    protected Vector3 _controlPoint;
    protected float _time;
    protected float _approachDuration;

    protected int TID;

    public virtual void Init(Damage damage, float radius, float approachDuration, Vector3 startPosition, Transform targetTransform, int TID)
    {
        _damage = damage;
        _radius = radius;
        transform.position = startPosition;
        _approachDuration = approachDuration;

        _startPosition = startPosition;
        _targetTransform = targetTransform;
        _time = 0f;
        this.TID = TID;

        float distance = Vector3.Distance(_startPosition, _targetTransform.position);
        _controlPoint = GetRandomBezierControlPoint(_startPosition, _targetTransform.position, Mathf.Clamp(distance - 2, 0.5f, distance / 2), distance);
    }


    private Vector3 GetRandomBezierControlPoint(Vector3 start, Vector3 end, float minDistance, float maxDistance)
    {
        float angle = Random.Range(-90f, 90f);
        float distance = Random.Range(minDistance, maxDistance);

        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

        Vector3 controlPoint = start + direction * distance + Vector3.up * Random.Range(1f, 3f);

        return controlPoint;
    }

    protected Vector3 GetQuadraticBezierPoint(float time, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float oneMinusT = 1f - time;
        return oneMinusT * oneMinusT * p0
         + 2f * oneMinusT * time * p1
         + time * time * p2;
    }

    public abstract void Update();
}
