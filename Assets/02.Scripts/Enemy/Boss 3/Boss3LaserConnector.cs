using UnityEngine;

public class Boss3LaserConnector : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private LaserScript laserScript;

    private void Start()
    {
        laserScript.firePoint = firePoint.gameObject;
        laserScript.endPoint = endPoint.gameObject;
        laserScript.ShootLaser(9999f); // 사실상 무한 발사
    }

    private void Update()
    {
        // 봉이 휘두르든 안휘두르든 계속 갱신
        laserScript.UpdateLaser();
    }
}
