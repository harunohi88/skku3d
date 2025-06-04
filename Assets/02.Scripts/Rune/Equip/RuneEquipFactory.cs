using System;
using UnityEngine;
using System.Collections.Generic;

public class RuneEquipFactory : Singleton<RuneEquipFactory>
{
    private readonly Dictionary<string, Func<ARuneEquip>> _registry = new();
    
    public void Register(string key, Func<ARuneEquip> constructor)
    {
        _registry[key] = constructor;
    }

    public ARuneEquip CreateRuneEquip(string key)
    {
        if (_registry.TryGetValue(key, out Func<ARuneEquip> constructor))
        {
            return constructor();
        }
        
        return null;
    }
}