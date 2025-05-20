using UnityEngine;
using System.Collections.Generic;
using System;

public class CooldownManager : BehaviourSingleton<CooldownManager>
{
    private List<CooldownEntry> _activeCooldownList = new List<CooldownEntry>();

    private void Update()
    {
        for (int i = _activeCooldownList.Count - 1; i >= 0; i--)
        {
            CooldownEntry cooldownEntry = _activeCooldownList[i];
            cooldownEntry.CooldownTime -= Time.deltaTime;
            if (cooldownEntry.CooldownTime <= 0f)
            {
                cooldownEntry.OnCooltimeEnd?.Invoke();
                _activeCooldownList.RemoveAt(i);
            }
        }
    }

    public void StartCooldown(float duration, Action onCooldownEnd)
    {
        _activeCooldownList.Add(new CooldownEntry
        {
            CooldownTime = duration,
            OnCooltimeEnd = onCooldownEnd
        });   
    }
}
