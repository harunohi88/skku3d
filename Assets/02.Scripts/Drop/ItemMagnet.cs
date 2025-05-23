using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    public float Movespeed = 5f;
    private Transform _playerTransform;
    private bool _isMoving = false;

    public GameObject MovingParent;

    public void Start()
    {
        MovingParent = gameObject.transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ma");
        if(other.gameObject.CompareTag("Player"))
        {
            _playerTransform = other.transform;
            _isMoving = true;
        }
    }

    void Update()
    {
        if (_isMoving && _playerTransform != null)
        {
            MovingParent.transform.position = Vector3.MoveTowards(
                transform.position,
                _playerTransform.position,
                Time.deltaTime * Movespeed
            );
        }
    }
}
