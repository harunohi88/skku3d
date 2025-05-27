using UnityEngine;

[CreateAssetMenu(fileName = "OnSkillUse", menuName = "Scriptable Objects/OnSkillUse")]
public class OnSkillUse : ARuneTrigger
{
    public float data;
    
    public override bool Trigger(RuneExecuteContext context)
    {
        return true;
    }
}