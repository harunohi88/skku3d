using UnityEngine;

public class Flame_DynamicRune : ADynamicRuneObject
{
    private Vector3 targetPosition;
    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
        targetPosition = targetTransform.position;
    }

    public override void Update()
    {
        // 화면 밖에서 안으로 떨어짐
    }
}
