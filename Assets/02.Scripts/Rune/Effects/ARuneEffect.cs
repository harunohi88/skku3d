public abstract class ARuneEffect
{
    public abstract void Initialize(RuneData data, int tier);
    public abstract void ApplyEffect(RuneExecuteContext context, ref Damage damage);
}
