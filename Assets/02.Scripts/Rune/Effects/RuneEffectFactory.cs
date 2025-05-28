using UnityEngine;
using System;
using System.Collections.Generic;

public class RuneEffectFactory : Singleton<RuneEffectFactory>
{
    private readonly Dictionary<string, Func<ARuneEffect>> _registry = new();
    
    public void Register(string key, Func<ARuneEffect> constructor)
    {
        _registry[key] = constructor;
    }

    public ARuneEffect CreateRuneEffect(string key)
    {
        if (_registry.TryGetValue(key, out Func<ARuneEffect> constructor))
        {
            return constructor();
        }
        
        return null;
    } 
}
