using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Missile : AEnemy
{
    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new IdleState());
    }

    public override void Attack()
    {
        EnemyRotation.IsFound = false;

        StartCoroutine(Attack_Coroutine());
    }

    public IEnumerator Attack_Coroutine()
    {
        float angleStep = 90f;
        Vector3[] directions = new Vector3[]
        {
            Quaternion.AngleAxis(-90f, transform.forward) * transform.up,
            transform.up,                                      
            Quaternion.AngleAxis(90f, transform.forward) * transform.up
        };

        List<Missile> missiles = new();

        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, 2f, transform.position.z) + directions[i] * 1.5f;
            Missile missile = Instantiate(SkillObject, spawnPos, Quaternion.identity, transform).GetComponent<Missile>();
            missiles.Add(missile);

            yield return new WaitForSeconds(0.5f);
        }

        for(int i = 0; i < missiles.Count; i++)
        {
            missiles[i].transform.parent = null;
            Damage damage = new Damage();
            damage.Value = Damage;
            damage.From = this.gameObject;
            missiles[i].Init(damage);
            yield return new WaitForSeconds(1f);
        }
    }
}