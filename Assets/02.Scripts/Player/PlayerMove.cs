using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Speed")]
    public float MoveSpeed;

    [Header("Roll")]
    public float RollSpeed;
    public float RollDuration;

    public GameObject Model;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector2 inputDirection)
    {
        if (inputDirection.sqrMagnitude < 0.01f)
            return;

        // 1. 카메라 기준 방향 추출 (수평 방향만 사용)
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // 2. 입력값을 카메라 방향 기준으로 변환
        Vector3 move = (camRight * inputDirection.x + camForward * inputDirection.y).normalized;

        // 3. 방향 전환 및 이동
        if (move != Vector3.zero)
        {
            Model.transform.forward = move;
            _characterController.Move(move * MoveSpeed * Time.deltaTime);
        }
    }

    public void Roll()
    {
        
    }
}
