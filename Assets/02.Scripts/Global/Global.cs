using UnityEngine;
using System;
using System.Collections;

public class Global : BehaviourSingleton<Global>
{
    public Action OnDataLoaded;
    public Vector2 MapMin;
    public Vector2 MapMax;
    private IEnumerator Start()
    {
        yield return DataTable.Instance.Load_Routine();
        OnDataLoaded?.Invoke();
    }
}
