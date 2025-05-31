using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float Gravity = -9.81f;

    public float RollDuration;
    public float RollAdditionalSpeed;
    private Vector3 _rollDirection;

    public GameObject Model;
    public PlayerManager PlayerManager;

    private Camera _mainCamera;
    private CharacterController _characterController;
    private Animator _animator;
    private float _verticalVelocity;
    private bool _isGrounded;
    private float _moveSpeed => PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MoveSpeed].TotalStat;
    private float _rollSpeed => PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MoveSpeed].TotalStat + RollAdditionalSpeed;
    private Vector3 _externalForce = Vector3.zero;
    public Vector3 LastMoveDirection { get; private set; } = Vector3.zero;

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

    public void ApplyExternalForce(Vector3 force)
    {
        _externalForce = force;
    }

    public void ClearExternalForce()
    {
        _externalForce = Vector3.zero;
    }

    public void Move(Vector2 inputDirection)
    {
        _animator.SetFloat("Movement", inputDirection.magnitude);

        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -1f; // 살짝 붙여주는 느낌
        }
        else
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }

        if (inputDirection.sqrMagnitude < 0.01f)
        {   
            LastMoveDirection = Vector3.zero;
            Vector3 fallOnly = new Vector3(0, _verticalVelocity, 0) + _externalForce;
            _characterController.Move(fallOnly * Time.deltaTime);
            return;
        }

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camRight * inputDirection.x + camForward * inputDirection.y).normalized;
        LastMoveDirection = move;

        if (move != Vector3.zero)
        {
            PlayerManager.PlayerState = EPlayerState.Move;
            // Model.transform.forward = move;
        }

        Vector3 totalMove = move * _moveSpeed;
        totalMove.y = _verticalVelocity;
        totalMove += _externalForce;

        _characterController.Move(totalMove * Time.deltaTime);
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
            _characterController.Move(_rollDirection * _rollSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _rollDirection = Vector3.zero;
        _animator.SetBool("isRolling", false);
        PlayerManager.Instance.PlayerState = EPlayerState.None;
        PlayerManager.PlayerAttack.Cancel();
    }
}
