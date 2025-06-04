using UnityEngine;

public class Boss3DieState : IState<AEnemy>
{
    private float _time = 0f;
    public void Enter(AEnemy enemy)
    {
        Debug.Log(this);

        AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss3Die);
        enemy.SetAnimationTrigger("Die");
        enemy.Agent.ResetPath();
        enemy.EnemyRotation.IsFound = false;
        Boss3AIManager.Instance.PortalToNextStage.SetActive(true);
        

        DropTable.Instance.DropBossRewards(enemy.transform.position, enemy.DropPosition, 10f);
    }

    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if(_time > enemy.DeathTime)
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }

    public void Exit(AEnemy enemy)
    {
        enemy.EnemyRotation.IsFound = true;
    }
}