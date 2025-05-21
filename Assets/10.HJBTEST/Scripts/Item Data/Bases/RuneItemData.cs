using UnityEngine;
using UnityEditor;

namespace Rito.InventorySystem
{
    public class RuneItemData : ItemData
    {
        public RuneData RuneData { get; private set; }

        public RuneItemData(RuneData runeData)
        {
            RuneData = runeData;
            ID = runeData.TID;
            Name = runeData.RuneName;
            Tooltip = runeData.RuneDescription;
            
            // 스프라이트 로드
            string spritePath = $"Assets/04.Images/{runeData.RuneName}.png";
            IconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        }

        public override Item CreateItem()
        {
            return new RuneItem(this);
        }
    }
} 