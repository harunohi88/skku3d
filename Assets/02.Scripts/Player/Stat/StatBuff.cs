using UnityEngine;

public class StatBuff
{
    public EStatType BuffStatType;
    public EBuffType BuffType;
    public float BuffValue;
    public float Duration;
    public bool IsPermanent;
    public int SourceTid;

    public StatBuff(EStatType buffStatType, EBuffType buffType, float buffValue, float duration, bool isPermanent, int tid)
    {
        BuffStatType = buffStatType;
        BuffType = buffType;
        BuffValue = buffValue;
        Duration = duration;
        IsPermanent = isPermanent;
        SourceTid = tid;
    }
    
    public bool IsValid(float deltaTime)
    {
        if (IsPermanent)
        {
            return true;
        }

        Duration -= deltaTime;
        
        if (Duration <= 0)
        {
            return false;
        }
        
        return true;
    }

    public void RefreshTime(float duration)
    {
        Duration = duration;
    }
    // Permanent Buff에 대한 삭제 처리 메서드 필요함
}
