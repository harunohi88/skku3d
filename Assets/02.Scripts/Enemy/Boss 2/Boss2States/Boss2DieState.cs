using UnityEngine;

public class Boss2DieState : IState<AEnemy>
{
    private float _time = 0f;

    public void Enter(AEnemy enemy)
    {
        enemy.SetAnimationTrigger("Die");
        AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss2Die);

        enemy.Agent.ResetPath();
        Boss2AIManager.Instance.PortalToNextStage.SetActive(true);

        DropTable.Instance.DropBossRewards(enemy.transform.position, enemy.DropPosition, 10f);
    }
    public void Update(AEnemy enemy)
    {
        _time += Time.deltaTime;
        if (_time > enemy.DeathTime)
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }

    public void Exit(AEnemy enemy)
    {

    }
}
