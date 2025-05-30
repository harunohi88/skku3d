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
    protected float _moveSpeed;
    protected float _bezierLength;

    protected int TID;

    public virtual void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        _damage = damage;
        _radius = radius;
        transform.position = startPosition;
        _moveSpeed = moveSpeed;

        _startPosition = startPosition;
        _targetTransform = targetTransform;
        _time = 0f;
        this.TID = TID;

        Vector3 targetPosition = new Vector3(_targetTransform.position.x, 1f, _targetTransform.position.z);

        float distance = Vector3.Distance(_startPosition, targetPosition);
        _controlPoint = GetRandomBezierControlPoint(_startPosition, targetPosition, Mathf.Clamp(distance - 2, 0.5f, distance / 2), distance);
        _bezierLength = EstimateBezierLength(_startPosition, _controlPoint, targetPosition);
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

    private float EstimateBezierLength(Vector3 p0, Vector3 p1, Vector3 p2, int samples = 10)
    {
        float length = 0f;
        Vector3 prev = p0;

        for (int i = 1; i <= samples; i++)
        {
            float t = i / (float)samples;
            Vector3 point = GetQuadraticBezierPoint(t, p0, p1, p2);
            length += Vector3.Distance(prev, point);
            prev = point;
        }

        return length;
    }

    public abstract void Update();
}
