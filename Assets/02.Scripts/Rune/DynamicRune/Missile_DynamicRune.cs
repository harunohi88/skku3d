using UnityEngine;

public class Missile_DynamicRune : ADynamicRuneObject
{
    public float InitialPhaseDuration = 0.7f;
    public float InitialSpeed = 3f;
    public float FinalSpeed = 20f;
    public float DirectionChangeInterval = 0.1f;
    public float WobbleAngle = 30f;

    public ParticleSystem Booster;
    public GameObject BoomPrefab;

    private float _elapsed = 0f;
    private bool _isHoming = false;

    private float _dirTimer = 0f;
    private Vector3 _currentDir;
    private Vector3 _targetDir;

    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
        _dirTimer = 0f;
        _elapsed = 0f;
        _isHoming = false;
    }

    public override void Update()
    {
        _elapsed += Time.deltaTime;

        if (!_isHoming)
        {
            _dirTimer += Time.deltaTime;
            if (_dirTimer > DirectionChangeInterval)
            {
                _dirTimer = 0f;
                _targetDir = GetRandomDirection();
            }

            _currentDir = Vector3.Slerp(_currentDir, _targetDir, Time.deltaTime * 5f);
            transform.position += _currentDir.normalized * InitialSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(_currentDir);

            if (_elapsed >= InitialPhaseDuration)
            {
                _isHoming = true;
                _elapsed = 0f;
                Booster.Play();
                _startPosition = transform.position;
            }
        }
        else
        {
            float t = _elapsed * FinalSpeed / Vector3.Distance(_startPosition, _targetTransform.position);
            t = Mathf.Clamp01(t);
            Vector3 pos = GetQuadraticBezierPoint(t, _startPosition, _controlPoint, _targetTransform.position);
            transform.position = pos;

            Vector3 forward = (GetQuadraticBezierPoint(t + 0.01f, _startPosition, _controlPoint, _targetTransform.position) - pos).normalized;
            if (forward != Vector3.zero) transform.rotation = Quaternion.LookRotation(forward);

            if(t >= 0.95f)
            {
                Vector3 position = new Vector3(_targetTransform.position.x, 0.3f, _targetTransform.position.z);
                Instantiate(BoomPrefab, position, Quaternion.identity);

                Debug.Log("반지름 매직넘버");
                Collider[] colliders = Physics.OverlapSphere(transform.position, 3.5f, LayerMask.GetMask("Enemy"));
                for (int i = 0; i < colliders.Length; i++)
                {
                    Damage newDamage = new Damage();
                    newDamage.Value = _damage.Value;
                    newDamage.From = _damage.From;
                    RuneManager.Instance.CheckCritical(ref newDamage);
                    colliders[i].GetComponent<AEnemy>()?.TakeDamage(newDamage);
                }

                RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
            }
        }
    }

    private Vector3 GetRandomDirection()
    {
        // 전방 기준으로 위쪽(30~80도)에 해당하는 방향으로 휘청거리게 설정
        float verticalAngle = Random.Range(30f, 80f); // 상향 각도 범위
        float horizontalAngle = Random.Range(-40f, 40f); // 좌우 휘청 범위

        Quaternion rot = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
        return rot * transform.forward;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.GetInstanceID() == _targetTransform.gameObject.GetInstanceID())
    //    {
    //        Damage newDamage = new Damage();
    //        newDamage.Value = Damage.Value;
    //        newDamage.From = Damage.From;
    //        RuneManager.Instance.CheckCritical(ref newDamage);

    //        other.GetComponent<AEnemy>().TakeDamage(newDamage);
    //    }
    //}
}
