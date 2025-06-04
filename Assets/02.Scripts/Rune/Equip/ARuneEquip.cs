public abstract class ARuneEquip
{
    public StatBuff EquipBuff;
    public abstract void Initialize(RuneData data, int tier);
    
    public void OnEquip()
    {
        BuffManager.Instance.AddBuff(EquipBuff);
    }

    public void OnUnequip()
    {
        BuffManager.Instance.RemoveBuff(EquipBuff);
    }
}
