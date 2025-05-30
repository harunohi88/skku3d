using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public GameObject Model;
    public LayerMask GroundLayer;
    public float LookAtMouseDuration;

    private Camera _mainCamera;
    private PlayerManager _playerManager;

    private bool _isLockedToMouse;
    private float _lockElapsed;

    private Vector3 _inputDirection;

    private void Start()
    {
        _mainCamera = Camera.main;
        _playerManager = PlayerManager.Instance;
    }

    public void Rotate(Vector2 inputDirection)
    {
        if (_isLockedToMouse || _playerManager.PlayerSkill.IsTargeting)
        {
            Vector3 mouseDir = GetMouseDirection();
            if (mouseDir != Vector3.zero)
            {
                Model.transform.forward = mouseDir;
            }
            
            _lockElapsed += Time.deltaTime;
            if (_lockElapsed >= LookAtMouseDuration)
            {
                _isLockedToMouse = false;
            }
        }
        else
        {
            Vector3 camForward = _mainCamera.transform.forward;
            Vector3 camRight = _mainCamera.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Model.transform.forward = (camRight * inputDirection.x + camForward * inputDirection.y).normalized;
        }
    }

    public void LookAtMouseForDuration()
    {
        _isLockedToMouse = true;
        _lockElapsed = 0f;
    }

    private Vector3 GetMouseDirection()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, GroundLayer))
        {
            Vector3 dir = hit.point - transform.position;
            dir.y = 0;
            return dir.normalized;
        }
        return Vector3.zero;
    }

    public void InstantLookAtMouse()
    {
        Vector3 mouseDir = GetMouseDirection();
        if (mouseDir != Vector3.zero)
        {
            Model.transform.forward = mouseDir;
        }

        LookAtMouseForDuration();
    }
}