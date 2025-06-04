using UnityEngine;

public class Boss2DieState : IState<AEnemy>
{
    private float _time = 0f;

    public void Enter(AEnemy enemy)
    {
        AudioManager.Instance.PlayEnemyAudio(EnemyType.Boss, EnemyAudioType.Boss2Die);
        enemy.SetAnimationTrigger("Die");
        enemy.Agent.ResetPath();
        Boss2AIManager.Instance.PortalToNextStage.SetActive(true);

        DropTable.Instance.Drop(enemy.Type, enemy.transform.position, 3);
        int n = Random.Range(2, 5);
        for (int i = 0; i < n; i++)
        {
            int randomTier = Random.Range(1, 3);
            DropTable.Instance.DropRandomRune(enemy.transform.position, enemy.Type, randomTier);
        }
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
