using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Collections;

public class Electric_DynamicRune : ADynamicRuneObject
{
    public List<LineRenderer> LineRendererList;
    public GameObject HitObject;

    private Transform _startTransform;
    private List<Transform> _targetTransformList;
    private float _duration;

    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
    }

    public void InitElectric(Damage damage, float radius, float moveSpeed, List<Transform> targetTransformList, float duration, int positionSize, int TID)
    {
        base.Init(damage, radius, moveSpeed, targetTransformList[0].position, targetTransformList[0], TID);
        _targetTransformList = targetTransformList;

        _duration = duration;
        for(int i = 0; i < LineRendererList.Count; i++)
        {
            LineRendererList[i].positionCount = positionSize;
        }
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _duration)
        {
            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
            return;
        }

        if (_targetTransform == null)
        {
            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
            return;
        }

        UpdateLaser();
    }

    public void UpdateLaser()
    {
        if(_targetTransformList[0].gameObject != null)
        {
            for (int i = 1; i < _targetTransformList.Count; i++)
            {

                Vector3 position = new Vector3(_targetTransformList[i].transform.position.x, 0.3f, _targetTransformList[i].transform.position.z);
                GameObject electric = GameObject.Instantiate(HitObject, position, Quaternion.identity);

                Vector3 position1 = new Vector3(_targetTransformList[i - 1].position.x, 1f, _targetTransformList[i - 1].position.z);
                Vector3 position2 = new Vector3(_targetTransformList[i].position.x, 1f, _targetTransformList[i].position.z);

                for (int j = 0; j < LineRendererList.Count; j++)
                {
                    LineRendererList[j].SetPosition(i - 1, position1);
                    LineRendererList[j].SetPosition(i, position2);
                }
            }
        }
    }
}
