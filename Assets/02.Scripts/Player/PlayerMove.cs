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

    private Camera _mainCamera;
    private CharacterController _characterController;
    private Animator _animator;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _characterController = GetComponent<CharacterController>();
        _animator = Model.GetComponent<Animator>();
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

    public void Roll()
    {
        _animator.SetTrigger("Roll");
        StartCoroutine(RollCoroutine());
    }

    private IEnumerator RollCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < RollDuration)
        {
            _characterController.Move(Model.transform.forward * RollSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
