using UnityEngine;

public class DieState : IState<AEnemy>
{
    private float _time;

    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("Die");
        enemy.Agent.ResetPath();
        enemy.EnemyRotation.IsFound = false;
        _time = 0;
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if(_time >= enemy.DeathTime)
        {
            enemy.ThisSpawner.ActivedEnemyCountDecrease();
            enemy.GetComponent<Collider>().enabled = false;
            switch (enemy.Type)
            {
                case EnemyType.Basic:
                    BasicEnemyPool.Instance.Return(enemy);
                    DropTable.Instance.DropRandomRune(enemy.transform.position, enemy.Type);
                    break;
                case EnemyType.Elite:
                    EliteEnemyPool.Instance.Return(enemy);
                    DropTable.Instance.DropRandomRune(enemy.transform.position, enemy.Type);
                    break;
            }
            EnemyTracker.Unregister(enemy.transform);

            //enemy.gameObject.SetActive(false);
            Debug.Log("사라짐");
        }
    }

    public void Exit(AEnemy enemy)
    {

    }
}
