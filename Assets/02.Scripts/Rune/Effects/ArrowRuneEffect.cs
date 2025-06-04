using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class ArrowRuneEffect : ARuneEffect
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
        RuneManager.Instance.StartCoroutine(Arrow_Coroutine(context, damage));   
    }

    public IEnumerator Arrow_Coroutine(RuneExecuteContext context, Damage damage)
    {
        List<Collider> colliderList = Physics.OverlapSphere(context.Player.transform.position, 10f, LayerMask.GetMask("Enemy")).ToList();
        Damage DamageBase = new Damage();
        DamageBase.Value = damage.Value * _damageMultiplier;
        DamageBase.From = damage.From;

        if (colliderList.Count != 0)
        {
            int n = 4 + (int)PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.ProjectileCountGain].TotalStat;
            for (int i = 0; i <= n; i++)
            {
                int index = Random.Range(0, colliderList.Count);
                Transform targetTransform = colliderList[index].transform;

                Vector3 offset = Quaternion.Euler(0, (360f / n) * i, 0) * (-context.Player.transform.forward * 1.5f);
                Vector3 spawnPos = context.Player.transform.position + offset + Vector3.up * 1f;
                Arrow_DynamicRune dyRune = RuneManager.Instance.ProjectilePoolDic[_tid].Get() as Arrow_DynamicRune;
                dyRune.gameObject.SetActive(false);

                dyRune.Init(DamageBase, 0, 12f, spawnPos, targetTransform, _tid);
                dyRune.gameObject.SetActive(true);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
