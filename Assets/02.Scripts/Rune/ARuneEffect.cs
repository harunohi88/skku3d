using UnityEngine;

public abstract class ARuneEffect : ScriptableObject
{
    public abstract void ApplyEffect(RuneExecuteContext context);
}
