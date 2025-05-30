using UnityEngine;

public class BasicInventorySelector : MonoBehaviour
{
    public BasicInventory Tier1Inventory;
    public BasicInventory Tier2Inventory;
    public BasicInventory Tier3Inventory;

    public GameObject Tier1Object;
    public GameObject Tier2Object;
    public GameObject Tier3Object;

    private void Start()
    {
        Tier1Object.SetActive(false);
        Tier2Object.SetActive(false);
        Tier3Object.SetActive(false);
    }
}
