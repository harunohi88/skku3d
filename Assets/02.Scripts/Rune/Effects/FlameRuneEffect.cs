using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlameRuneEffect : ARuneEffect
{
    private int _tid;
    private float _damageMultiplier;
    public override void Initialize(RuneData data, int tier)
    {
        _tid = data.TID;
        _damageMultiplier = data.TierList[tier - 1];
    }

    public override void ApplyEffect(RuneExecuteContext context, ref Damage damage)
    {
        List<Collider> colliderList = Physics.OverlapSphere(context.Player.transform.position, 12f, LayerMask.GetMask("Enemy")).ToList();
        Damage DamageBase = new Damage();
        DamageBase.Value = damage.Value *_damageMultiplier;
        DamageBase.From = damage.From;

        if (colliderList.Count != 0)
        {

            int index = Random.Range(0, colliderList.Count);
            Transform targetTransform = colliderList[index].transform;
            Vector3 spawnPos = GetMeteorSpawnPosition(targetTransform, 10, 30);
            Flame_DynamicRune dyRune = RuneManager.Instance.ProjectilePoolDic[_tid].Get() as Flame_DynamicRune;
            dyRune.gameObject.SetActive(false);

            dyRune.transform.forward = (targetTransform.position - spawnPos).normalized;
            dyRune.Init(DamageBase, 0, 3, spawnPos, targetTransform, _tid);

            dyRune.gameObject.SetActive(true);
        }
    }

    private Vector3 GetMeteorSpawnPosition(Transform enemy, float radius = 4f, float angleDegree = 20f)
    {
        float theta = Mathf.Deg2Rad * angleDegree;
        float phi = Random.Range(0, 360f) * Mathf.Deg2Rad;

        float x = radius * Mathf.Sin(theta) * Mathf.Cos(phi);
        float y = radius * Mathf.Cos(theta);
        float z = radius * Mathf.Sin(theta) * Mathf.Cos(phi);

        return enemy.position + new Vector3(x, y, z);
    }
}
