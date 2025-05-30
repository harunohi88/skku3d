using UnityEngine;

namespace suhyeon
{
    public class TopDownCameraFollow_SH : MonoBehaviour
    {
        public Transform target;       // ���� �÷��̾�
        public Vector3 offset = new Vector3(0, 15f, -10f); // ������ �������� ����
        public float followSpeed = 10f;

        void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}
