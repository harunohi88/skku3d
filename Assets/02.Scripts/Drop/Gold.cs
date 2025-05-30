using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] private int _goldAmount = 1;
    public int GoldAmount { get => _goldAmount; set => _goldAmount = value; }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // TODO: 골드 매니저나 게임 매니저를 통해 골드 추가
            // GameManager.Instance.AddGold(_goldAmount);
            Debug.Log($"Gold collected: {_goldAmount}");
            Destroy(gameObject);
        }
    }

    //TODO: 골드 추가
}
