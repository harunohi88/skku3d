using System;
using UnityEngine;

public class TimeManager : BehaviourSingleton<TimeManager>
{
    public int Difficulty;
    public int TimeInSecond;

    public Action OnDifficultyChanged;
}
