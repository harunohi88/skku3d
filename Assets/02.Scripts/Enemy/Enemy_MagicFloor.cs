using UnityEngine;
using System.Collections;
using System.Linq;

public class Enemy_MagicFloor : AEnemy
{
    public GameObject IndicatorPrefab;
    public SkillIndicator Indicator;
    public float FloorRadius;
    public float CastingTime;

    public override void Init(EnemySpawner spawner)
    {
        base.Init(spawner);
        _stateMachine.ChangeState(new IdleState());
        if (Indicator == null) Indicator = Instantiate(IndicatorPrefab).GetComponent<SkillIndicator>();
        Indicator.gameObject.SetActive(false);
    }

    public override void Attack()
    {
        EnemyRotation.IsFound = false;
        Vector3 position = PlayerManager.Instance.Player.transform.position;

        Indicator.SetPosition(position);
        Indicator.CircularInit(FloorRadius * 2, FloorRadius * 2, 0, 360, 0, 0);
        Indicator.Ready(CastingTime);

        StartCoroutine(Floor_Coroutine(position));
    }

    public IEnumerator Floor_Coroutine(Vector3 position)
    {
        yield return new WaitForSeconds(CastingTime);
        Collider[] colliders = Physics.OverlapSphere(position, FloorRadius, LayerMask);

        if (colliders.Length > 0)
        {
            // 저장 위치로 때리기
        }
    }
}