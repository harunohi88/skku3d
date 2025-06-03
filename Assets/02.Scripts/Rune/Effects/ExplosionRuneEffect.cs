using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class ExplosionRuneEffect : ARuneEffect
{
    private int _tid;
    private float _damageMultiplier;

    private static bool _isSoundOn = false;
    public override void Initialize(RuneData data, int tier)
    {
        _tid = data.TID;
        _damageMultiplier = data.TierList[tier - 1];
    }

    public override void ApplyEffect(RuneExecuteContext context, ref Damage damage)
    {
        Damage DamageBase = new Damage();
        DamageBase.Value = damage.Value * _damageMultiplier;
        DamageBase.From = damage.From;

        Explosion_DynamicRune dyRune = RuneManager.Instance.ProjectilePoolDic[_tid].Get() as Explosion_DynamicRune;
        dyRune.gameObject.SetActive(false);

        Vector3 spawnPos = context.TargetEnemy.transform.position + Vector3.up * 0.5f;
        dyRune.Init(DamageBase, 2, 12f, spawnPos, context.TargetEnemy.transform, _tid);

        dyRune.gameObject.SetActive(true);

        if (_isSoundOn == false) RuneManager.Instance.StartCoroutine(Sound_Coroutine());
    }

    public IEnumerator Sound_Coroutine()
    {
        _isSoundOn = true;
        AudioManager.Instance.PlayDynamicRuneAudio(DynamicRuneAudioType.Explosion1);

        yield return new WaitForSeconds(0.1f);

        _isSoundOn = false;
    }
}
