using UnityEngine;

public class GameManager : BehaviourSingleton<GameManager>
{
    private int _currentStage;

    public int GetCurrentStage() => _currentStage;
}
