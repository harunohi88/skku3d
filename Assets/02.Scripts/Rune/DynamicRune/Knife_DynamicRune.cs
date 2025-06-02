using UnityEngine;

public class Knife_DynamicRune : ADynamicRuneObject
{
    public GameObject HitObject;
    public GameObject TrailObject;
    private float InitialPhaseDuration = 0.3f;
    private float _elapsedPhaseTime = 0f;
    private bool _isTrailOn = false;

    private bool _isStabbing;
    public int MaxStabCount = 3;
    private int StabCount = 0;

    public Vector3 RotationSpeed = new Vector3(10f, 25f, 30f);

    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
        _isStabbing = false;
        StabCount = 0;
        _time = 0;

        _elapsedPhaseTime = 0f;
        TrailObject.SetActive(false);
        _isTrailOn = false;
    }

    public override void Update()
    {
        Vector3 targetPosition = new Vector3(_targetTransform.position.x, 2f, _targetTransform.position.z);
        transform.Rotate(RotationSpeed * Time.deltaTime);

        if (_isStabbing == false)
        {
            float moveStep = _moveSpeed * Time.deltaTime;
            float timeStep = moveStep / _bezierLength;
            _time += timeStep;

            Vector3 currentPos = GetQuadraticBezierPoint(_time, _startPosition, _controlPoint, targetPosition);
            transform.position = currentPos;

            if (_time >= 1f)
            {
                _isStabbing = true;
                _time = 0f;
            }

            if (_isTrailOn == false)
            {
                _elapsedPhaseTime += Time.deltaTime;
                if (_elapsedPhaseTime >= InitialPhaseDuration)
                {
                    _isTrailOn = true;
                    TrailObject.SetActive(true);
                }
            }
        }
        else
        {
            _time += Time.deltaTime * 2f;
            float angle = _time * Mathf.PI * 2;
            float x = Mathf.Sin(angle);
            float y = Mathf.Sin(angle) * Mathf.Cos(angle);
            Vector3 offset = new Vector3(x * 2f, 0, y * 2f);
            transform.position = targetPosition + offset;
        }

        if (_targetTransform.gameObject.activeSelf == false)
        {
            TrailObject.SetActive(false);
            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetInstanceID() == _targetTransform.gameObject.GetInstanceID())
        {
            Damage newDamage = new Damage();
            newDamage.Value = _damage.Value;
            newDamage.From = _damage.From;
            RuneManager.Instance.CheckCritical(ref newDamage);

            other.GetComponent<AEnemy>()?.TakeDamage(newDamage);
            StabCount++;

            if(StabCount >= MaxStabCount)
            {
                Ray ray = new Ray(transform.position - transform.forward * 0.5f, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 2f, LayerMask.GetMask("Enemy")))
                {
                    Vector3 hitPoint = hitInfo.point;
                    Vector3 hitNormal = hitInfo.normal;

                    Quaternion rot = Quaternion.LookRotation(hitNormal); // normal을 기준으로 회전
                    Instantiate(HitObject, hitPoint, rot);
                }

                TrailObject.SetActive(false);
                RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
            }
        }
    }
}
