using UnityEngine;
using System.Collections.Generic;
using System;

public class CooldownManager : BehaviourSingleton<CooldownManager>
{
    public List<CooldownEntry> ActiveCooldownList = new List<CooldownEntry>();

    private void Update()
    {
        for (int i = ActiveCooldownList.Count - 1; i >= 0; i--)
        {
            CooldownEntry cooldownEntry = ActiveCooldownList[i];
            cooldownEntry.CooldownTime -= Time.deltaTime;
            UIEventManager.Instance.OnCooldown?.Invoke(
                cooldownEntry.SkillIndex,
                cooldownEntry.CooldownTime,
                cooldownEntry.MaxCooldownTime
                );
            if (cooldownEntry.CooldownTime <= 0f)
            {
                cooldownEntry.OnCooltimeEnd?.Invoke();
                ActiveCooldownList.RemoveAt(i);
            }
        }
    }

    public void StartCooldown(int index, float maxDuration, float duration, Action onCooldownEnd)
    {
        ActiveCooldownList.Add(new CooldownEntry
        {
            SkillIndex = index,
            MaxCooldownTime = maxDuration,
            CooldownTime = duration,
            OnCooltimeEnd = onCooldownEnd
        });   
    }
}
