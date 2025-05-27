public abstract class ARuneTrigger
{
    public abstract void Initialize(RuneData data);
    public abstract bool Trigger(RuneExecuteContext context);
}
