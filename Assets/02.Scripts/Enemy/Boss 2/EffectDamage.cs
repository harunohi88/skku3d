using UnityEngine;

public class EffectDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Damage damage = new Damage();
            damage.Value = Boss2AIManager.Instance.GetPatternData(0).Damage;
            PlayerManager.Instance.Player.TakeDamage(damage);
            Debug.Log("Player Hit 판정");
        }
    }
}
