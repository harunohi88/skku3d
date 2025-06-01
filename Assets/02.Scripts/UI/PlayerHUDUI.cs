using UnityEngine;
using Microlight.MicroBar;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerHUDUI : MonoBehaviour
{
    public MicroBar HealthBar;
    public MicroBar StaminaBar;
    public MicroBar ExpBar;

    public List<Image> CooldownImageList;
    public List<TextMeshProUGUI> CooldownTextList;
    public List<bool> IsCooldownList = new List<bool>(4);

    private void Start()
    {
        Global.Instance.OnDataLoaded += Init;

        UIEventManager.Instance.OnStatChanged += ChangeStat;
        UIEventManager.Instance.OnSkillUse += SKillCooldown;
    }

    public void Init()
    {
        HealthBar.Initialize(PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxHealth].TotalStat);
        StaminaBar.Initialize(PlayerManager.Instance.PlayerStat.StatDictionary[EStatType.MaxStamina].TotalStat);
        // 경험치 초기화
        // ExpBar.Initialize(PlayerManager.Instance.경험치.TotalStat);

        UIEventManager.Instance.OnStatChanged += ChangeStat;
    }

    // 체력 변동 후, 스태미나 변동 후 (차오를때도 계속), 경험치 변동 후 호출
    // MicroBar가 연속적인 처리는 렉걸린것처럼 늦게 반응하더라구요 스태미나 회복할때 이거 문제생기면 말씀해주세요 내일 해볼게요
    public void ChangeStat()
    {
        HealthBar.UpdateBar(PlayerManager.Instance.Player.Health, false);
        // 스태미나 부분
        //StaminaBar.UpdateBar(PlayerManager.현재 스태미나, false);
        // 경험치 부분
        //ExpBar.UpdateBar(PlayerManager.Instance.현재 경험치);
    }

    public void SKillCooldown()
    {
        // 인덱스 가져오기 or currentSkill 들고와서 자신의 index 확인
        // UIEventManager에서 OnSkillUse 액션을 <int>형으로 받고 현재 스킬 인덱스 매개변수로 가져오고 싶은데
        // 아까 스킬 2개에 이미 OnSkillUse 심어놔서 일단 안바꿨습니다. 편한 방식으로 수정해주세요

        //if (IsCooldownList[index])
        //{
        //    StartCoroutine(Cooldown_coroution(index));
        //}
       
    }

    public IEnumerator Cooldown_coroution(int index)
    {
        // 스킬 슬롯은 0이 스킬 1인데 여긴 0이 기본공격
        IsCooldownList[index + 1] = true;
        CooldownImageList[index + 1].gameObject.SetActive(true);
        CooldownTextList[index + 1].gameObject.SetActive(true);
        //float Cooldown = 스킬 쿨타임;
        //float _time = Cooldown;
        //while (_time >= 0)
        //{
        //    // fillAmount 1에서 0으로 
        //    CooldownImageList[index + 1].fillAmount = _time / Cooldown;
        //    CooldownTextList[index + 1].text = _time.ToString("0.0");
        //    _time -= Time.deltaTime;
            yield return null;
        //}

        IsCooldownList[index + 1] = false;
        CooldownImageList[index + 1].gameObject.SetActive(false);
        CooldownTextList[index + 1].gameObject.SetActive(false);
    }
}
