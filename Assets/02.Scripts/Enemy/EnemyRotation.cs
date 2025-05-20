using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    public bool IsFound = false;
    public float RotationSpeed;
    void Update()
    {
        if (IsFound)
        {
            //transform.LookAt(PlayerManager.Instance.Player.transform.position);
            Vector3 directionToPlayer = PlayerManager.Instance.Player.transform.position - transform.position;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), RotationSpeed * Time.deltaTime);
        }
    }
}
