using NUnit.Framework;
using RayFire;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ElectricRuneEffect : ARuneEffect
{
    private int _tid;
    private int _enemyCount;

    public override void Initialize(RuneData data, int tier)
    {
        _tid = data.TID;
        _enemyCount = (int)data.TierList[tier - 1];
    }

    public override void ApplyEffect(RuneExecuteContext context, ref Damage damage)
    {
        List<Collider> colliderList = Physics.OverlapSphere(context.Player.transform.position, 20f, LayerMask.GetMask("Enemy")).ToList();
        Damage DamageBase = new Damage();
        DamageBase.Value = damage.Value * 0.3f;
        DamageBase.From = damage.From;

        if (colliderList.Count != 0)
        {
            List<Transform> randomList = colliderList
                                        .OrderBy(x => Random.value)
                                        .Take(Mathf.Min(_enemyCount, colliderList.Count))
                                        .Select(collider => collider.transform)
                                        .ToList();

            if(randomList.Count > 1)
            {
                Electric_DynamicRune dyRune = RuneManager.Instance.ProjectilePoolDic[_tid].Get() as Electric_DynamicRune;
                dyRune.gameObject.SetActive(false);

                dyRune.InitElectric(DamageBase, 0, 0, randomList, 2f, randomList.Count, _tid);
                dyRune.gameObject.SetActive(true);
            }

            RuneManager.Instance.StartCoroutine(Slow_Coroutine(2f, randomList));

            for (int i = 0; i < randomList.Count; i++)
            {
                Damage newDamage = new Damage();
                newDamage.Value = DamageBase.Value;
                newDamage.From = DamageBase.From;
                RuneManager.Instance.CheckCritical(ref newDamage);

                randomList[i].GetComponent<AEnemy>().TakeDamage(newDamage);
            }
        }
    }

    public IEnumerator Slow_Coroutine(float duration, List<Transform> enemyList)
    {

        for(int i = 0; i < enemyList.Count; i++)
        {
            AEnemy enemy = enemyList[i].GetComponent<AEnemy>();
            enemy.Agent.speed /= 2f;
        }

        yield return new WaitForSeconds(duration);

        for(int i = 0; i < enemyList.Count; i++)
        {
            AEnemy enemy = enemyList[i].GetComponent<AEnemy>();
            enemy.Agent.speed = enemy.MoveSpeed;
        }
    }
}
