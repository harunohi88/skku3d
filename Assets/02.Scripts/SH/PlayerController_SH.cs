using UnityEngine;

namespace suhyeon
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController_SH : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float runSpeed = 9f;
        public float rotationSpeed = 720f;

        [Header("Jump Settings")]
        public float jumpForce = 7f;
        public float gravity = -20f;

        private CharacterController characterController;
        private Camera mainCamera;

        private Vector3 velocity;
        private bool isGrounded;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            // 바닥 체크
            isGrounded = characterController.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // 바닥에 닿았을 때 안정적인 접지
            }

            // WASD 이동
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 moveDir = new Vector3(h, 0, v).normalized;

            if (moveDir.magnitude >= 0.1f)
            {
                float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
                Vector3 move = transform.right * moveDir.x + transform.forward * moveDir.z;
                characterController.Move(move * currentSpeed * Time.deltaTime);
            }

            // 스페이스바 → 점프
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = jumpForce;
            }

            // 중력 적용
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        private void HandleRotation()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float hitDistance))
            {
                Vector3 lookPoint = ray.GetPoint(hitDistance);
                Vector3 direction = (lookPoint - transform.position);
                direction.y = 0;

                if (direction.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
