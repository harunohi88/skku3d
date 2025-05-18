using UnityEngine;

public class DieState : IState<AEnemy>
{
    private float _time;

    public void Enter(AEnemy enemy)
    {
        enemy.Agent.ResetPath();
        _time = 0;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if(_time >= enemy.DeathTime)
        {
            Debug.Log("사라짐");
        }
    }

    public void Exit(AEnemy enemy)
    {

    }
}
