using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class SlaughterRuneEffect : ARuneEffect
{
    private int _tid;
    private int _stabCount;
    public override void Initialize(RuneData data, int tier)
    {
        _tid = data.TID;
        _stabCount = (int)data.TierList[tier - 1];
    }

    public override void ApplyEffect(RuneExecuteContext context, ref Damage  damage)
    {
        List<Collider> colliderList = Physics.OverlapSphere(context.Player.transform.position, 10f, LayerMask.GetMask("Enemy")).ToList();
        Damage DamageBase = new Damage();
        DamageBase.Value = damage.Value * 0.5f;
        DamageBase.From = damage.From;

        if (colliderList.Count != 0)
        {
            for (int i = 0; i <= PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.ProjectileCountGain].TotalStat; i++)
            {
                Damage newDamage = new Damage();
                newDamage.Value = DamageBase.Value;
                newDamage.From = DamageBase.From;
                RuneManager.Instance.CheckCritical(ref newDamage);

                int index = Random.Range(0, colliderList.Count);
                Transform targetTransform = colliderList[index].transform;

                Vector3 offset = Quaternion.Euler(0, (360f / _stabCount) * i, 0) * (-context.Player.transform.forward * 1.5f);
                Vector3 spawnPos = context.Player.transform.position + offset + Vector3.up * 1f;
                Knife_DynamicRune dyRune = RuneManager.Instance.ProjectilePoolDic[_tid].Get() as Knife_DynamicRune;
                dyRune.gameObject.SetActive(false);

                dyRune.Init(newDamage, 0, 10f, spawnPos, targetTransform, _tid);
                dyRune.gameObject.SetActive(true);

                dyRune.MaxStabCount = _stabCount;
            }
        }
    }
}
