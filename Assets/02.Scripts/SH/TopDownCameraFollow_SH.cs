using UnityEngine;

namespace suhyeon
{
    public class TopDownCameraFollow_SH : MonoBehaviour
    {
        public Transform target;       // 따라갈 플레이어
        public Vector3 offset = new Vector3(0, 15f, -10f); // 위에서 내려보는 시점
        public float followSpeed = 10f;

        void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}
