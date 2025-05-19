using UnityEngine;
using System.Collections.Generic;

public class Boids : MonoBehaviour
{
    [SerializeField]private float _forwardSpeed;
    [SerializeField]private float _searchRadius_cohesion_alignment;
    [SerializeField]private float _searchRadius_separation;
    [SerializeField]private float _rotateSpeed;

    [SerializeField]private float _targetWeight;
    [SerializeField]private float _alignmentWeight = 1.0f;
    [SerializeField]private float _cohesionWeight = 1.0f;
    [SerializeField]private float _separationWeight = 1.0f;

    public Transform Target;
    private List<Collider> _colliderList = new List<Collider>();

    public void getTarget(Transform target)
    {
        Target = target;
    }

    // Update is called once per frame
    void Update()
    {
        moveBoid();
    }

    void moveBoid()
    {
        // 범위내 콜라이더 확인
        if (_colliderList.Count > 0)
        {
            _colliderList.Clear();
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius_cohesion_alignment);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemy"))
            {
                _colliderList.Add(colliders[i]);
            }
        }
        colliders = _colliderList.ToArray();


        // cohesion, alignment, separation 저장할 변수 초기화
        Vector3 totalCenter = Vector3.zero;
        Vector3 totalDir = Vector3.zero;
        Vector3 totalSeparation = Vector3.zero;
        Vector3 BoidDir = Vector3.zero;
        Vector3 cohesionDirection = Vector3.zero;
        Vector3 Vec_toTarget = Vector3.zero;
        int totalSeparationCount = 0;

        foreach (Collider collider in colliders)
        {
            // alignment 무리 방향으로 조정
            totalDir += collider.transform.forward;

            // cohesion 무리 중심 위치로 이동
            totalCenter += collider.bounds.center;

            // separation 무리와 충돌하지 않게 반대 방향으로 이동
            if (Vector3.Distance(transform.position, collider.transform.position) <= _searchRadius_separation)
            {
                totalSeparation += transform.position - collider.transform.position;
            }
            totalSeparationCount++;
        }

        if(totalSeparationCount > 0)
        {
            totalDir /= colliders.Length;
            totalCenter /= colliders.Length;
            cohesionDirection = totalCenter - transform.position;

            Vec_toTarget = Target.position - transform.position;
            totalDir += _targetWeight * Vec_toTarget.normalized;

            BoidDir = _alignmentWeight * totalDir + _cohesionWeight * cohesionDirection + _separationWeight * totalSeparation;
            BoidDir.Normalize();

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(BoidDir), _rotateSpeed * Time.deltaTime);
        }
        transform.position += transform.forward * _forwardSpeed * Time.deltaTime;
    }
}
