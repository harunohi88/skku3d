using UnityEngine;

public class Arrow_DynamicRune : ADynamicRuneObject
{
    public GameObject HitObject;
    public GameObject SmokeTrail;
    private float InitialPhaseDuration = 0.3f;
    private float _elapsedPhaseTime = 0f;
    private bool _isTrailOn = false;

    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
        _elapsedPhaseTime = 0f;
        SmokeTrail.SetActive(false);
        _isTrailOn = false;

        AudioManager.Instance.PlayDynamicRuneAudio(DynamicRuneAudioType.Fly1);
    }

    public override void Update()
    {
        float moveStep = _moveSpeed * Time.deltaTime;
        float timeStep = moveStep / _bezierLength;
        _time += timeStep;

        if(_isTrailOn == false)
        {
            _elapsedPhaseTime += Time.deltaTime;
            if (_elapsedPhaseTime >= InitialPhaseDuration)
            {
                _isTrailOn = true;
                SmokeTrail.SetActive(true);
            }
        }

        Vector3 targetPosition = new Vector3(_targetTransform.position.x, _targetTransform.position.y + 0.5f, _targetTransform.position.z);

        Vector3 currentPos = GetQuadraticBezierPoint(_time, _startPosition, _controlPoint, targetPosition);
        transform.position = currentPos;

        // 방향 회전
        float lookAhead = 0.01f; // 약간 앞의 지점으로 향하게
        float tLook = Mathf.Clamp01(_time + lookAhead);
        Vector3 lookPos = GetQuadraticBezierPoint(tLook, _startPosition, _controlPoint, targetPosition);

        Vector3 direction = (lookPos - currentPos).normalized;
        transform.forward = direction;

        if (_time >= 1f)
        {
            SmokeTrail.SetActive(false);
            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == _targetTransform.gameObject.GetInstanceID())
        {
            AudioManager.Instance.PlayDynamicRuneAudio(DynamicRuneAudioType.ArrowHit);
         
            Damage newDamage = new Damage();
            newDamage.Value = _damage.Value;
            newDamage.From = _damage.From;
            RuneManager.Instance.CheckCritical(ref newDamage);

            other.GetComponent<AEnemy>()?.TakeDamage(newDamage);

            Ray ray = new Ray(transform.position - transform.forward * 0.5f, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 2f, LayerMask.GetMask("Enemy")))
            {
                Vector3 hitPoint = hitInfo.point;
                Vector3 hitNormal = hitInfo.normal;

                Quaternion rot = Quaternion.LookRotation(hitNormal); // normal을 기준으로 회전
                Instantiate(HitObject, hitPoint, rot);
            }

            SmokeTrail.SetActive(false);
            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
        }
    }
}
