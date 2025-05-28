using Microlight.MicroBar;
using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    public AEnemy enemy;
    public MicroBar bar;

    private void Start()
    {
        enemy = GetComponentInParent<AEnemy>();
        bar.Initialize(enemy.MaxHealth);
        enemy.OnStatChanged += HealthChanged;
    }
    private void Update()
    {
        transform.forward = CameraManager.Instance.transform.forward;
    }

    public void HealthChanged()
    {
        bar.UpdateBar(enemy.Health, false, UpdateAnim.Damage);
    }
}
