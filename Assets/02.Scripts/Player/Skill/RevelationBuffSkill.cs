using UnityEngine;

public class RevelationBuffSkill : MonoBehaviour, ISkill
{
    public float CoolTime = 20f;
    public string SkillName = "RevelationBuff";
    private bool isBuffActive = false;

    public void Activate()
    {
        Debug.Log("Spin Slash Activated");
        if (isBuffActive) return;
        isBuffActive = true;
        // 플레이어 버프 구현
        // 1. 플레이어 버프 스탯값 조정 - 구조체
        // 공격력 +30%, 이동속도 +20%, 쿨감 +15%, 지속시간 10초

        // 일정 시간 후 버프 제거 - 코루틴



        // 애니메이션 구현
        // 스킬 vfx 추가
        // 사운드 추가
    }
}
