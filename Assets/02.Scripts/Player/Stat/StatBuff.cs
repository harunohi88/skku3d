using UnityEngine;

public class StatBuff : MonoBehaviour
{
    public EStatType BuffStatType;
    public EBuffType BuffType;
    public float BuffValue;
    public float Duration;
    public bool IsPermanent;

    public StatBuff(EStatType buffStatType, EBuffType buffType, float buffValue, float duration, bool isPermanent)
    {
        BuffStatType = buffStatType;
        BuffType = buffType;
        BuffValue = buffValue;
        Duration = duration;
        IsPermanent = isPermanent;
    }
    
    public bool IsValid()
    {
        if (IsPermanent)
        {
            return true;
        }
        
        Duration -= Time.deltaTime;
        
        if (Duration <= 0)
        {
            return false;
        }
        
        return true;
    }
    
    // Permanent Buff에 대한 삭제 처리 메서드 필요함
}
