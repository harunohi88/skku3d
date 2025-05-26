using System;
using UnityEngine;
using System.Collections.Generic;

public static class RuneFactory
{
    private static Dictionary<int, Func<int, ARune>> _registry = new();
 
    public static void RegisterRune(int tid, Func<int, ARune> constructor)
    {
        if (_registry.ContainsKey(tid))
        {
            Debug.LogWarning($"TID {tid} already registered.");
            return;
        }

        _registry.Add(tid, constructor);
    }

    public static ARune CreateRune(int tid, int tier)
    {
        if (!_registry.TryGetValue(tid, out var constructor))
        {
            throw new Exception($"Rune with TID {tid} is not registered.");
        }

        return constructor(tier);
    }
}
