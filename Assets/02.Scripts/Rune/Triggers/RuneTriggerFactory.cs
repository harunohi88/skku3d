using System;
using UnityEngine;
using System.Collections.Generic;

public class RuneTriggerFactory : Singleton<RuneTriggerFactory>
{
    private readonly Dictionary<string, Func<ARuneTrigger>> _registry = new();
    
    public void Register(string key, Func<ARuneTrigger> constructor)
    {
        _registry[key] = constructor;
    }

    public ARuneTrigger CreateRuneTrigger(string key)
    {
        if (_registry.TryGetValue(key, out Func<ARuneTrigger> constructor))
        {
            return constructor();
        }
        
        return null;
    }
}
