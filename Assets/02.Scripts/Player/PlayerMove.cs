using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Speed")]
    public float MoveSpeed;

    [Header("Roll")]
    public float RollSpeed;
    public float RollDuration;
    private Vector3 _rollDirection;

    public GameObject Model;
    public PlayerManager PlayerManager;

    private Camera _mainCamera;
    private CharacterController _characterController;
    private Animator _animator;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _characterController = GetComponent<CharacterController>();
        _animator = Model.GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerManager = PlayerManager.Instance;
    }

    public void Move(Vector2 inputDirection)
    {
        _animator.SetFloat("Movement", inputDirection.magnitude);

        if (inputDirection.sqrMagnitude < 0.01f)
        {
            _characterController.Move(Vector3.zero);
            return;
        }

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camRight * inputDirection.x + camForward * inputDirection.y).normalized;

        if (move != Vector3.zero)
        {
            Model.transform.forward = move;
            _characterController.Move(move * MoveSpeed * Time.deltaTime);
        }
    }

    public void Roll(Vector2 direction)
    {
        PlayerManager.PlayerState = EPlayerState.Roll;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        _rollDirection = (camRight * direction.x + camForward * direction.y).normalized;
        if (_rollDirection == Vector3.zero)
        {
            _rollDirection = Model.transform.forward;
        }
        else
        {
            Model.transform.forward = _rollDirection;
        }
        _animator.SetTrigger("Roll");
        _animator.SetBool("isRolling", true);
        StartCoroutine(RollCoroutine());
    }

    private IEnumerator RollCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < RollDuration)
        {
            _characterController.Move(_rollDirection * RollSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _rollDirection = Vector3.zero;
        _animator.SetBool("isRolling", false);
        PlayerManager.Instance.PlayerState = EPlayerState.None;
        PlayerManager.PlayerAttack.Cancel();
    }
}
