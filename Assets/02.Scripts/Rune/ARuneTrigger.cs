using UnityEngine;

public abstract class ARuneTrigger : ScriptableObject
{
    public abstract bool Trigger(RuneExecuteContext context);
}
