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

    public override void ApplyEffect(RuneExecuteContext context)
    {
        List<Collider> colliderList = Physics.OverlapSphere(context.Player.transform.position, 10f, LayerMask.NameToLayer("Enemy")).ToList();
        
        if(colliderList.Count != 0)
        {
            int index = Random.Range(0, colliderList.Count);
            Transform targetTransform = colliderList[index].transform;

            Debug.Log("투사체 개수 가져와야됨");
            for (int i = 0; i < 1; i++)
            {
                Vector3 offset = Quaternion.Euler(0, (360f / 1), 0) * (-context.Player.transform.forward * 1.5f);
                Vector3 spawnPos = context.Player.transform.position + offset + Vector3.up * 1f;
                Knife_DynamicRune dyRune = RuneManager.Instance.ProjectilePoolDic[_tid].Get() as Knife_DynamicRune;
                dyRune.Init(context.Damage, 0, 2f, spawnPos, targetTransform);
                dyRune.StabCount = 3;
            }
        }
    }
}
