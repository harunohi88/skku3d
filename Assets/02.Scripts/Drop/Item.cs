using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public enum EItemType
{
    Rune,
    Exp,
    Coin,
}

[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    public float InitialBounceHeight = 2f;
    public float BounceDuration = 0.3f;
    public int BounceCount = 3;
    public float RotationSpeed = 90f;

    public float AttractRange = 3f;
    public float RetreatDistance = 2f;
    public float RetreatDuration = 0.1f;
    public float GetDuration = 0.25f;

    public EItemType Type;
    public Rune Rune;
    public int Amount;

    public bool IsCollected = false;
    public List<GameObject> SparkleEffectList;
    private int _tier = 1;

    private Sequence bounceSeq;

    private Transform _player => PlayerManager.Instance.Player.transform;

    public BasicAllInventory BasicAllInventory;

    private void Start()
    {
        BasicAllInventory = GameObject.FindAnyObjectByType<BasicAllInventory>();

        BounceEffect();
    }

    private void Update()
    {
        if (!IsCollected) transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
    }

    public void Init(int tier, int amount, Rune rune, EItemType type)
    {
        _tier = tier;
        Type = type;
        Rune = rune;
        Amount = amount;

        for (int i = 0; i < SparkleEffectList.Count; i++)
        {
            if (i < _tier) SparkleEffectList[i].SetActive(true);
            else SparkleEffectList[i].SetActive(false);
        }
        BounceEffect();
    }

    private void BounceEffect()
    {
        if (transform == null) return;

        Vector3 startPos = transform.position;

        // 1. 최초 발사 방향 계산
        Vector3 randomDir = (Vector3.up + Random.onUnitSphere * 0.5f).normalized;
        Vector3 initialTarget = startPos + randomDir * InitialBounceHeight;

        bounceSeq = DOTween.Sequence().SetAutoKill(true);

        // 2. 초기 발사
        bounceSeq.Append(transform.DOMove(initialTarget, BounceDuration / 2f).SetEase(Ease.OutQuad));

        // 3. 바운스 반복 (3회)
        Vector3 currentPos = initialTarget;
        float bouncePower = InitialBounceHeight;

        for (int i = 0; i < BounceCount; i++)
        {
            // 3-1. 튀어오르는 힘 감소
            bouncePower *= 0.5f;

            // 3-2. 랜덤 XZ 추가 (점점 작게)
            Vector3 randomXZ = new Vector3(
                Random.Range(-0.5f, 0.5f) * bouncePower,
                0f,
                Random.Range(-0.5f, 0.5f) * bouncePower
            );

            // 3-3. 새로운 최고점 위치
            Vector3 bouncePeak = currentPos + Vector3.up * bouncePower + randomXZ;

            // 3-4. 최고점으로 올라가기
            bounceSeq.Append(transform.DOMove(bouncePeak, BounceDuration / 2f).SetEase(Ease.OutQuad));

            // 3-5. 바닥으로 떨어지기 (Y=시작지점 Y)
            Vector3 floorPos = new Vector3(bouncePeak.x, startPos.y, bouncePeak.z);
            bounceSeq.Append(transform.DOMove(floorPos, BounceDuration / 2f).SetEase(Ease.InQuad));

            currentPos = floorPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsCollected) return;
        if (other.CompareTag("Player")) StartCoroutine(AttractToPlayer());
    }

    private IEnumerator AttractToPlayer()
    {
        DOTween.Kill(transform);

        IsCollected = true;

        Vector3 direction = (transform.position - _player.position).normalized;
        Vector3 retreatPos = transform.position + direction * RetreatDistance;

        Tween retreatTween = transform.DOMove(retreatPos, RetreatDuration).SetEase(Ease.OutQuad);
        yield return retreatTween.WaitForCompletion();

        // null 체크
        if (transform == null) yield break;

        Tween moveToPlayerTween = transform.DOMove(_player.position, GetDuration).SetEase(Ease.InQuad);
        yield return moveToPlayerTween.WaitForCompletion();

        if (transform == null) yield break;
        switch (Type)
        {
            case EItemType.Rune:
                AudioManager.Instance.PlayPlayerAudio(PlayerAudioType.GetRune);
                BasicAllInventory.AddItem(Rune);
                break;
            case EItemType.Exp:
                int audioTypeEnumIndex = (int)PlayerAudioType.GetExp1;
                AudioManager.Instance.PlayPlayerAudio((PlayerAudioType)((int)Random.Range(audioTypeEnumIndex, audioTypeEnumIndex + 4)));
                PlayerManager.Instance.PlayerLevel.GainExperience((float)Amount);
                break;
            case EItemType.Coin:
                AudioManager.Instance.PlayPlayerAudio(PlayerAudioType.GetCoin);
                CurrencyManager.Instance.AddGold(Amount);
                break;
        }

        DOTween.Kill(transform, complete: true);
        if (bounceSeq != null && bounceSeq.IsActive())
        {
            bounceSeq.Kill(complete: true);
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        DOTween.Kill(transform, complete: true);
        if (bounceSeq != null && bounceSeq.IsActive())
        {
            bounceSeq.Kill(complete: true);
        }
    }
}
