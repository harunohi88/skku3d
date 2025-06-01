using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : BehaviourSingleton<InventoryManager>
{
    public List<Sprite> RuneSpriteList;
    private const int RUNE_SPRITE_START_INDEX = 10000;

    public Tooltip ToolTip;

    public Sprite GetSprite(int tid)
    {
        return RuneSpriteList[tid - RUNE_SPRITE_START_INDEX];
    }
}
