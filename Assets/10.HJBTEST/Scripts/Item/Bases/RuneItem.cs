using UnityEngine;

namespace Rito.InventorySystem
{
    public class RuneItem : Item, IUsableItem
    {
        public RuneItemData RuneItemData => Data as RuneItemData;
        public RuneData RuneData => RuneItemData.RuneData;

        public RuneItem(RuneItemData data) : base(data)
        {
        }

        public bool Use()
        {
            // TODO: Implement rune usage logic
            Debug.Log($"Using rune: {RuneData.RuneName}");
            return true;
        }
    }
} 