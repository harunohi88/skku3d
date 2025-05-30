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
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    public Transform Center;
    private Vector3 _originPos;
    private Vector3 _startPos;
    private float _elapsed = 0f;

    // Lift 상태: 위로 빠르게 상승
    [Header("Lift Movement")]
    public float liftHeight = 3f;
    public float liftSpeed = 5f;
    private bool _isLifting = false;

    // Attack 상태: 회전
    [Header("Attack Movement")]
    public float spinSpeed = 360f;
    public float tiltAngle = 45f;

    public float RotationAngle = 180f;

    private void OnEnable()
    {

        _originPos = transform.position;
        _startPos = _originPos;
        _elapsed = 0f;
        _isLifting = false;
    }

    private void Update()
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
        // y축 공전
        transform.RotateAround(Center.position, Vector3.up, spinSpeed * Time.deltaTime);

        // 봉 자체를 수평으로 눕힘
        transform.rotation = Quaternion.LookRotation(Center.position - transform.position); // 중심을 향하도록 방향 설정
        transform.Rotate(Vector3.right, RotationAngle); // X축 회전으로 수평 누이기
    }

    public void SetState(WeaponState newState)
    {
        CurrentState = newState;
        if (newState == WeaponState.Idle)
        {
            _startPos = _originPos;
            _elapsed = 0;
        }
        else if (newState == WeaponState.Lift)
        {
            _startPos = _originPos;
            _elapsed = 0;
            _isLifting = false;
        }
    }
}
