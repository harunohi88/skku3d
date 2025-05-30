using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class FateRuneEffect : ARuneEffect
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
        List<Collider> colliderList = Physics.OverlapSphere(context.Player.transform.position, 10f, LayerMask.GetMask("Enemy")).ToList();
        Damage DamageBase = new Damage();
        DamageBase.Value = damage.Value * _damageMultiplier;
        DamageBase.From = damage.From;

        if (colliderList.Count != 0)
        {
            int n = 1 + (int)PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.ProjectileCountGain].TotalStat;
            for (int i = 0; i <= n; i++)
            {
                int index = Random.Range(0, colliderList.Count);
                Transform targetTransform = colliderList[index].transform;

                Vector3 spawnPos = context.Player.transform.position + Vector3.up * 3f;
                Missile_DynamicRune dyRune = RuneManager.Instance.ProjectilePoolDic[_tid].Get() as Missile_DynamicRune;
                dyRune.gameObject.SetActive(false);

                dyRune.Init(DamageBase, 0, 12f, spawnPos, targetTransform, _tid);
                dyRune.gameObject.SetActive(true);
            }
        }
    }
}
