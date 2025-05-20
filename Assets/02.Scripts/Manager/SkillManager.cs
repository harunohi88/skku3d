using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    private Dictionary<string, float> skillCooldowns = new()
    {
        {"SpinSlash", 5f },
        {"JumpStrike", 6.5f },
        {"RevelationBuff", 20f}
    };
    private Dictionary<string, float> currentCooldowns = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 
}
