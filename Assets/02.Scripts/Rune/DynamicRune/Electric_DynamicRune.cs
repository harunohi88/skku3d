using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class Electric_DynamicRune : ADynamicRuneObject
{
    private Transform _startTransform;
    private List<ParticleSystem> _childParticleList;
    public float _duration;

    private void Awake()
    {
        _childParticleList = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            _childParticleList.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        }
    }

    public override void Init(Damage damage, float radius, float moveSpeed, Vector3 startPosition, Transform targetTransform, int TID)
    {
        base.Init(damage, radius, moveSpeed, startPosition, targetTransform, TID);
    }

    public void InitElectric(Damage damage, float radius, float moveSpeed, Transform startTransform, Transform targetTransform, int TID)
    {
        for (int i = 0; i < _childParticleList.Count; i++)
        {
            _childParticleList[i].Stop();
        }

        base.Init(damage, radius, moveSpeed, startTransform.position, targetTransform, TID);
        _startTransform = startTransform;

        float distance = Vector3.Distance(_startTransform.position, _targetTransform.position);

        for (int i = 0; i < _childParticleList.Count; i++)
        {
            var ps = _childParticleList[i].main;
            ps.startSizeY = distance;
            _childParticleList[i].Play();
        }
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if(_time >= _duration)
        {
            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
            return;
        }

        if(_startTransform == null || _targetTransform == null)
        {
            RuneManager.Instance.ProjectilePoolDic[TID].Return(this);
            return;
        }
        Vector3 position1 = new Vector3(_targetTransform.position.x, 1f, _targetTransform.position.z);
        Vector3 position2 = new Vector3(_startTransform.position.x, 1f, _startTransform.position.z);

        transform.position = (position2 + position1) / 2f;

        Vector3 targetUp = (position1 - position2).normalized;
        Vector3 camPos = Camera.main.transform.position;
        Vector3 toCam = (camPos - transform.position).normalized;
        transform.up = targetUp;

        float distance = Vector3.Distance(_startTransform.position, _targetTransform.position);

        for(int i = 0; i < _childParticleList.Count; i++)
        {
            var ps = _childParticleList[i].main;
            ps.startSizeY = distance;

        }
    }
}
