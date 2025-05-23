using UnityEngine;

public class DieState : IState<AEnemy>
{
    private float _time;

    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("Die");
        enemy.Agent.ResetPath();
        _time = 0;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if(_time >= enemy.DeathTime)
        {
            enemy.ThisSpawner.ActivedEnemyCountDecrease();
            switch (enemy.Type)
            {
                case EnemyType.Basic:
                    BasicEnemyPool.Instance.Return(enemy);
                    break;
                case EnemyType.Elite:
                    EliteEnemyPool.Instance.Return(enemy);
                    break;
            }


            enemy.gameObject.SetActive(false);
            Debug.Log("사라짐");
        }
    }

    public void Exit(AEnemy enemy)
    {

    }
}
