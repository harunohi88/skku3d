using UnityEngine;
using System.Collections.Generic;

public class PlayerEquipment : MonoBehaviour
{
    public List<Rune> RuneList;

    private void Start()
    {
        RuneList = new List<Rune>(2) {null, null};
    }
    public void EquipRune(int slot, Rune rune)
    {
        if (RuneList[slot] != null)
        {
            UnequipRune(slot);
        }
        
        RuneList[slot] = rune;
        RuneList[slot].EquipRune();
    }

    public void UnequipRune(int slot)
    {
        if (RuneList[slot] == null) return;
        
        RuneList[slot].UnequipRune();
        RuneList[slot] = null;
    }

    public void RuneEffectExecute(RuneExecuteContext context, ref Damage damage)
    {
        foreach (Rune rune in RuneList)
        {
            if (rune != null && rune.CheckTrigger(context))
            {
                rune.ApplyEffect(context, ref damage);
            }
        }
    }
}
