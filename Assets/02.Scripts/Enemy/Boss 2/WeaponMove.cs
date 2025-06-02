using UnityEngine;


public class WeaponMove : MonoBehaviour
{
    // Idle 상태일 때 봉이 위아래로 움직인다. 봉은 세로로 세워져 있다.(붕붕 뜨는 느낌)
    // Lift 상태일 때 봉이 위로 확 올라간다. 봉은 세로로 세워져 있다.
    // Attack 상태일 때 도넛 모양의 인디케이터 범위 위에서 빠르게 360도 돌면서 크게 회전한다.
    public enum WeaponState
    {
        Idle,
        Lift,
        Throw
    }

    public WeaponState CurrentState = WeaponState.Idle;

    // Idle 상태: 붕붕 떠오르는 움직임
    [Header("Idle Movement")]
    public float floatAmplitude = 5f;
    public float floatFrequency = 5f;

    private Vector3 _originPos;
    private Vector3 _startPos;
    private float _elapsed = 0f;

    // Lift 상태: 위로 빠르게 상승
    [Header("Lift Movement")]
    public float liftHeight = 5f;
    public float liftSpeed = 5f;
    private bool _isLifting = false;

    // Attack 상태: 회전
    [Header("Attack Movement")]
    public float spinSpeed = 360f;
    public float tiltAngle = 45f;
    public float ThrowHeight = 1f;
    private float _deltaTime = 0f;

    public float RotationAngle = 90f;

    private void OnEnable()
    {

        _originPos = transform.position; // 손 앞에서 시작한다.
        _startPos = _originPos; // 손앞에서 시작한다.
        _elapsed = 0f;
        _isLifting = false;
    }

    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case WeaponState.Idle:
                HandleIdle();
                break;

            case WeaponState.Lift:
                HandleLift();
                break;

            case WeaponState.Throw:
                Debug.Log("Throw 상태");
                HandleAttack();
                break;
        }
    }



    private void HandleIdle()
    {
        _elapsed += Time.deltaTime * floatFrequency;
        Vector3 floatOffset = new Vector3(0, Mathf.Sin(_elapsed) * floatAmplitude, 0);
        transform.position = _startPos + floatOffset;
    }

    private void HandleLift()
    {
        if (!_isLifting)
        {
            _startPos = transform.position;
            _isLifting = true;
        }

        Vector3 target = _startPos + Vector3.up * liftHeight;
        transform.position = Vector3.MoveTowards(transform.position, target, liftSpeed * Time.deltaTime);
    }

    private void HandleAttack()
    {
        Debug.Log("봉아 돌아라 돌아라");
        EnemyPatternData boss2SpecialAttack2 = GetComponentInParent<Boss2AIManager>()._boss2SpecialAttack2PatternList[0];
        float radius = ((boss2SpecialAttack2.Radius * boss2SpecialAttack2.InnerRange) + boss2SpecialAttack2.Radius) / 4f;
        _deltaTime += Time.deltaTime;
        //transform.Rotate(0, 0, _deltaTime * 90);
        transform.rotation = Quaternion.Euler(90f, 0f, _deltaTime * 3600f);
        transform.position = new Vector3(
            transform.parent.position.x + radius * Mathf.Cos(_deltaTime * Mathf.Deg2Rad * 720f),
            transform.parent.position.y + ThrowHeight,
            transform.parent.position.z + radius * Mathf.Sin(_deltaTime * Mathf.Deg2Rad * 720f));
    }

    public void SetState(WeaponState newState)
    {
        CurrentState = newState;
        _deltaTime = 0f;
        _elapsed = 0f;

        if (newState == WeaponState.Idle)
        {
            transform.rotation = Quaternion.identity;
            _startPos = transform.position;

        }
        if (newState == WeaponState.Lift)
        {
            transform.rotation = Quaternion.identity;
            _startPos = transform.position;
            _isLifting = false;
        }
        else if (newState == WeaponState.Throw)
        {
            //_startPos = transform.parent.position;
            _elapsed = 0f;
        }
    }
}
