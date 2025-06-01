using System;
using UnityEngine;

public class UIEventManager : BehaviourSingleton<UIEventManager>
{
    public Action OnStatChanged;
    public Action OnSkillUse;
    public Action OnLevelUp;
}
