using UnityEngine;

public class OnSkillUseTrigger : ARuneTrigger
{
    public override void Initialize(RuneData data)
    {
    }

    public override bool Trigger(RuneExecuteContext context)
    {
        if (context.Skill == null)
        {
            return false;
        }

        return true;
    }
}
