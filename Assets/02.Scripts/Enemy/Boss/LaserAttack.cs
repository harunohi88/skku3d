using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    public Damage Damage;
    public Vector3 OriginPositon;
    public float FollowSpeed = 2f;

    public void Init()
    {
        transform.localPosition = OriginPositon;
    }

    private void Update()
    {
        Vector3 direction = (PlayerManager.Instance.Player.transform.position - transform.position).normalized;
        direction.y = 0f;  // y축 고정 (선택)

        transform.position += direction * FollowSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.Player.TakeDamage(Damage);
        }
    }
}
