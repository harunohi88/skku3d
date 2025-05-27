using UnityEngine;

public class StaffSpinTest : MonoBehaviour
{
    public Transform staff; // 회전시킬 봉
    public float spinDuration = 1f;       // 회전 시간
    public float spinAngle = 360f;        // 회전 각도 (한 바퀴)

    private bool isSpinning = false;
    private float elapsed = 0f;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Update()
    {
        // 테스트용 키 입력
        if (Input.GetKeyDown(KeyCode.Space) && !isSpinning)
        {
            StartSpin();
        }

        if (isSpinning)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / spinDuration);
            staff.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            if (t >= 1f)
            {
                isSpinning = false;
            }
        }
    }

    public void StartSpin()
    {
        initialRotation = staff.rotation;
        // 봉의 로컬 Y축 기준으로 360도 회전
        targetRotation = initialRotation * Quaternion.Euler(0f, 0f, spinAngle);
        elapsed = 0f;
        isSpinning = true;
    }
}
