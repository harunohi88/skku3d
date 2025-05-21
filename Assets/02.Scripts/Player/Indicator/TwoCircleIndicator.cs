using System;
using UnityEngine;
using System.Collections.Generic;

public class TwoCircleIndicator : MonoBehaviour
{
    private Player _player;
    
    [Header("Range")]
    public CircleAOE RangeIndicator;
    public Color RangeColor;
    public float RangeDirection;
    public float RangeAngle;
    public float RangeInnerRadius;
    public float RangeCastingPercentage;
    public float RangeRadius;
    
    
    [Header("Target")]
    public CircleAOE TargetIndicator;
    public Color TargetColor;
    public float TargetDirection;
    public float TargetAngle;
    public float TargetInnerRadius;
    public float TargetCastingPercentage;
    public float TargetRadius;
    public LayerMask GroundLayer;
    public float MaxRayDistance;

    private void Awake()
    {
        InitRangeIndicator();
        InitTargetIndicator();
    }

    private void Start()
    {
        _player = PlayerManager.Instance.Player;
    }

    private void Update()
    {
        RangeIndicator.transform.position = _player.transform.position;
        TargetIndicatorUpdate();
    }

    private void InitRangeIndicator()
    {
        RangeIndicator.SetColor(RangeColor);
        RangeIndicator.SetDirection(RangeDirection);
        RangeIndicator.SetAngle(RangeAngle);
        RangeIndicator.SetInnerRadius(RangeInnerRadius);
        RangeIndicator.SetCastingPercentage(RangeCastingPercentage);
        RangeIndicator.SetSize(RangeRadius);
    }

    private void InitTargetIndicator()
    {
        TargetIndicator.SetColor(TargetColor);
        TargetIndicator.SetDirection(TargetDirection);
        TargetIndicator.SetAngle(TargetAngle);
        TargetIndicator.SetInnerRadius(TargetInnerRadius);
        TargetIndicator.SetCastingPercentage(TargetCastingPercentage);
        TargetIndicator.SetSize(TargetRadius);
    }

    private void TargetIndicatorUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, MaxRayDistance, GroundLayer))
        { 
            TargetIndicator.transform.position = GetPointInRange(hit.point);
        }
    }

    private Vector3 GetPointInRange(Vector3 hitPoint)
    {
        Vector3 PointInRange = hitPoint;
        
        if (Vector3.Distance(hitPoint, RangeIndicator.transform.position) > RangeRadius)
        {
            Vector3 direction = (hitPoint - RangeIndicator.transform.position).normalized;
            PointInRange = RangeIndicator.transform.position + direction * RangeRadius;
        }
        
        return PointInRange;
    }
    
    public List<Collider> GetCollidersInTargetRange()
    {
        Collider[] colliders = Physics.OverlapSphere(TargetIndicator.transform.position, TargetRadius);
     
        List<Collider> colliderList = new List<Collider>(colliders);
        return colliderList;
    }
}
