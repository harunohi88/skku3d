using UnityEngine;

public class EXP : MonoBehaviour
{
    [SerializeField] private int _expAmount = 1;
    public int ExpAmount { get => _expAmount; set => _expAmount = value; }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // TODO: 경험치 매니저나 게임 매니저를 통해 골드 추가
            Debug.Log($"Gold collected: {_expAmount}");
            Destroy(gameObject);
        }
    }

    //TODO: 경험치 추가
}
