using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float PullStrength = 10f;
    public float MaxEffectRadius = 20f;
    public float MinEffectDistance = 2f;
    public float MinSpeedMultiplier = 0.5f;
    public float MaxSpeedMultiplier = 2f;

    private Transform _playerTransform;
    private PlayerMove _playerMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerTransform = PlayerManager.Instance.Player.transform;
        _playerMove = PlayerManager.Instance.PlayerMove;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerTransform == null || _playerMove == null) return;

        Vector3 toBlackHole = (transform.position - _playerTransform.position);
        toBlackHole.y = 0f; // Y축 영향 제거
        float distance = toBlackHole.magnitude;
        if (distance > MaxEffectRadius)
        {
            _playerMove.ClearExternalForce();
            return;
        }

        float clampedDistance = Mathf.Max(distance, MinEffectDistance);
        Vector3 toBlackHoleDir = toBlackHole.normalized;
        Vector3 playerVelocity = GetPlayerVelocity();
        playerVelocity.y = 0f; // Y축 영향 제거
        float towardDot = Vector3.Dot(playerVelocity.normalized, toBlackHoleDir);

        float speedMultiplier = 1f;
        if (playerVelocity.magnitude < 0.1f)
        {
            // 가만히 있으면 블랙홀 방향으로 끌려감
            speedMultiplier = 1f;
        }
        else if (towardDot > 0.5f)
        {
            // 블랙홀 방향으로 이동 중이면 가속
            speedMultiplier = Mathf.Lerp(1f, MaxSpeedMultiplier, towardDot);
        }
        else if (towardDot < -0.5f)
        {
            // 반대 방향이면 감속
            speedMultiplier = Mathf.Lerp(1f, MinSpeedMultiplier, -towardDot);
        }

        // 블랙홀 힘은 거리와 무관하게 일정한 힘
        float pull = PullStrength;
        Vector3 force = toBlackHoleDir * pull * speedMultiplier;
        force.y = 0f; // Y축 영향 제거
        _playerMove.ApplyExternalForce(force);
    }

    private Vector3 GetPlayerVelocity()
    {
        // PlayerMove의 LastMoveDirection을 사용
        return _playerMove.LastMoveDirection * PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MoveSpeed].TotalStat;
    }
}
