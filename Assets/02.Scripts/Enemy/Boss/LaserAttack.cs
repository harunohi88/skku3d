using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    public Damage Damage;
    public Vector3 OriginPositon;
    public float FollowSpeed = 2f;

    private float _durationTimer = 0f;
    private float _duration;
    private float _time = 0f;
    private float _timeTick = 0.2f;

    public void Init(float duration)
    {
        transform.localPosition = OriginPositon;
        _duration = duration;
        _durationTimer = 0f;
        _time = 0f;

        AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss1Sp2_2, true);
    }

    private void Update()
    {

        Vector3 direction = (PlayerManager.Instance.Player.transform.position - transform.position).normalized;
        direction.y = 0f;  // y축 고정 (선택)

        transform.position += direction * FollowSpeed * Time.deltaTime;

        _durationTimer += Time.deltaTime;
        if(_durationTimer < _duration)
        {
            _time += Time.deltaTime;
            if (_time >= _timeTick)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Player"));
                if (colliders.Length > 0)
                {
                    PlayerManager.Instance.Player.TakeDamage(Damage);
                }
                _time = 0f;
            }
        }
    }
}
