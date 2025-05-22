using UnityEngine;
using UnityEditor;

namespace Rito.InventorySystem
{
    public class RuneItemData : ItemData
    {
        public RuneData RuneData { get; private set; }

        public RuneItemData(RuneData runeData, int tier)
        {
            RuneData = runeData;
            ID = runeData.TID;
            Name = runeData.RuneName;
            
            string runeDescription = runeData.RuneDescription;
            Tier = tier;
            // 티어에 따라 계수 변환
            if(tier == 1)
            {
                RuneValue = runeData.TierList[0];
            }
            else if(tier == 2)
            {
                RuneValue = runeData.TierList[1];
            }
            else
            {
                RuneValue = runeData.TierList[2];
            }
            // 티어에 맞게 툴팁 변경
            runeDescription = runeDescription.Replace("N", RuneValue.ToString());
            Tooltip = runeDescription;

            // 스프라이트 로드
            string spritePath = $"Assets/04.Images/{runeData.RuneName}.png";
            IconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

            // TODO: 능력치 추가
        }

        public override Item CreateItem()
        {
            return new RuneItem(this);
        }
    }
} 