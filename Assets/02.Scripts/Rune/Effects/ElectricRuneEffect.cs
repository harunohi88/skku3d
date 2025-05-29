using NUnit.Framework;
using RayFire;
using System.Collections.Generic;
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
            List<Collider> randomList = colliderList.OrderBy(x => Random.value).Take(Mathf.Min(_enemyCount, colliderList.Count)).ToList();
            if(randomList.Count == 1)
            {
                // 라이트닝 생성하지 않고 감전효과만
            }
            else if(randomList.Count == 2)
            {
                // 라이트닝 하나 생성하고 두마리 감전효과
                Electric_DynamicRune dyRune = RuneManager.Instance.ProjectilePoolDic[_tid].Get() as Electric_DynamicRune;
                dyRune.gameObject.SetActive(false);

                dyRune.InitElectric(DamageBase, 0, 0, randomList[0].transform, randomList[1].transform, _tid);
                dyRune.gameObject.SetActive(true);
            }
            else
            {
                for(int i = 0; i < randomList.Count-1; i++)
                {
                    Electric_DynamicRune dyRune = RuneManager.Instance.ProjectilePoolDic[_tid].Get() as Electric_DynamicRune;
                    dyRune.gameObject.SetActive(false);

                    dyRune.InitElectric(DamageBase, 0, 0, randomList[i].transform, randomList[i+1].transform, _tid);
                    dyRune.gameObject.SetActive(true);
                }
            }

            for(int i = 0; i < randomList.Count; i++)
            {
                Vector3 position = new Vector3(randomList[i].transform.position.x, 0.3f, randomList[i].transform.position.z);
                GameObject electric = GameObject.Instantiate(RuneManager.Instance.ElectricEffectPrefab, position, Quaternion.identity);

                Damage newDamage = new Damage();
                newDamage.Value = DamageBase.Value;
                newDamage.From = DamageBase.From;
                RuneManager.Instance.CheckCritical(ref newDamage);

                randomList[i].GetComponent<AEnemy>().TakeDamage(newDamage);
            }
        }
    }
}
