using UnityEngine;

public class MoveSpeedBuffEffect : ARuneEffect
{
    private float _amount;
    private float _duration;
    private int _tid;
    
    public override void Initialize(RuneData data, int tier)
    {
        _amount = data.TierList[tier - 1];
        _duration = data.Time;
        _tid = data.TID;
    }

    public override void ApplyEffect(RuneExecuteContext context)
    {
        StatBuff buff = new StatBuff(
            EStatType.MoveSpeed,
            EBuffType.Multiply,
            _amount,
            _duration,
            false,
            _tid);
        BuffManager.Instance.AddBuff(buff);
    }
}
