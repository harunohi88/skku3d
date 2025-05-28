using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    private SphereCollider _sphereCollider;

    public float MoveSpeed = 5f;
    private Vector3 _direction;
    public float Radius = 0.5f;
    public LayerMask LayerMask;

    private float _destroyTime = 0f;
    private bool _isDestroyed = false;
    private bool _isShot = false;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    public void Init()
    {
        Vector3 PlayerPosition = new Vector3(PlayerManager.Instance.Player.transform.position.x, 1f, PlayerManager.Instance.Player.transform.position.z);
        _direction = (PlayerPosition - transform.position).normalized;
        _isShot = true;
    }

    private void Update()
    {
        if (_isDestroyed) return;

        if (_isShot)
        {
            transform.position += _direction * MoveSpeed * Time.deltaTime;

            RaycastHit hit;
            if (Physics.SphereCast(transform.position, Radius, transform.forward, out hit, Radius, LayerMask))
            {
                transform.position = hit.point;
                Instantiate(ExplosionPrefab, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal));

                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("데미지 줌");
                }

                _isDestroyed = true;
                _isShot = false;
                Destroy(gameObject);
                return;
            }

            _destroyTime += Time.deltaTime;
            if (_destroyTime >= 5f)
            {
                _isDestroyed = true;
                _isShot = false;
                Destroy(gameObject);
            }
        }

    }
}
