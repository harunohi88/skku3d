using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _player;

    private void Start()
    {
        _player = PlayerManager.Instance;
    }

    private void Update()
    {
        
    }

    private void Roll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, 0, vertical);
        move.Normalize();
    }
}
