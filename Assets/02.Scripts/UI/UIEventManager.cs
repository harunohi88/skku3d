using System;
using System.Collections.Generic;
using UnityEngine;

public class UIEventManager : BehaviourSingleton<UIEventManager>
{
    public Action OnStatChanged;
    public Action<int, float, float> OnCooldown;
    public Action<int, float> OnLevelUp;
    public Action<float> OnExpGain;
    public Action<int> OnSKillLevelUp;
    public Action<EStatType> OnStatUpgrade;
    public Action<int> OnUpgradePointChange;
    public Action<StatSnapshot> OnDisplayStatChanged;
    public Action<float> OnCurrentHealthChanged;
}
