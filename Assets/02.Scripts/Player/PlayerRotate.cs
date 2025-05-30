using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public GameObject Model;
    public LayerMask GroundLayer;
    public float LookAtMouseDuration;
    public float MouseLookSmoothSpeed = 10f;
    public float MouseLookInstantSpeed = 20f;
    public float InstantLookDuration = 0.1f; // 인스턴트 회전 시간
    private Coroutine _lookCoroutine;

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
    

    private void RotateToward(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Model.transform.rotation = Quaternion.Slerp(
            Model.transform.rotation,
            targetRotation,
            Time.deltaTime * MouseLookSmoothSpeed
        );
    }

    public void Rotate(Vector2 inputDirection)
    {
        if (_isLockedToMouse || _playerManager.PlayerSkill.IsTargeting)
        {
            Vector3 mouseDir = GetMouseDirection();
            RotateToward(mouseDir); // 부드럽게 회전
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

            Vector3 newForward = (camRight * inputDirection.x + camForward * inputDirection.y).normalized;
            if (newForward != Vector3.zero)
            {
                RotateToward(newForward); // 부드럽게 회전
            }
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

    // public void InstantLookAtMouse()
    // {
    //     Vector3 mouseDir = GetMouseDirection();
    //     if (mouseDir != Vector3.zero)
    //     {
    //         Model.transform.forward = mouseDir;
    //     }
    //
    // }
    
    public void InstantLookAtMouse()
    {
        if (_lookCoroutine != null)
        {
            StopCoroutine(_lookCoroutine);
        }

        LookAtMouseForDuration();
        _lookCoroutine = StartCoroutine(LookAtMouseSmooth());
    }

    private IEnumerator LookAtMouseSmooth()
    {
        float elapsed = 0f;
        Vector3 mouseDir = GetMouseDirection();
        if (mouseDir == Vector3.zero)
            yield break;

        Quaternion startRot = Model.transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(mouseDir);

        while (elapsed < InstantLookDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / InstantLookDuration;
            Model.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        Model.transform.rotation = targetRot;
        _lookCoroutine = null;
    }
    
    public void CancelRotation()
    {
        if (_lookCoroutine != null)
        {
            StopCoroutine(_lookCoroutine);
            _lookCoroutine = null;
        }
    }
}