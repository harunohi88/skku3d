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
    private bool _isAttractable = false;

    private Sequence bounceSeq;

    private Transform _player => PlayerManager.Instance.Player.transform;

    public BasicAllInventory BasicAllInventory;

    private void Start()
    {
        BasicAllInventory = GameObject.FindAnyObjectByType<BasicAllInventory>();
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
        _isAttractable = true;
        BounceEffect();
    }
    public void Init(int tier, int amount, Rune rune, EItemType itemType, Vector3 dropPosition, float radius)
    {
        _tier = tier;
        Type = itemType;
        Rune = rune;
        Amount = amount;

        for (int i = 0; i < SparkleEffectList.Count; i++)
        {
            if (i < _tier) SparkleEffectList[i].SetActive(true);
            else SparkleEffectList[i].SetActive(false);
        }
        _isAttractable = false;
        BounceEffect(dropPosition, radius);
    }

    private void BounceEffect()
    {
        if (transform == null) return;

        Vector3 startPos = transform.position;

        // 1. 최초 발사 방향 계산
        Vector3 randomDir = (Vector3.up + Random.onUnitSphere * 5f).normalized;
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

    private void BounceEffect(Vector3 targetPosition, float randomRadius = 1.0f)
    {
        if (transform == null) return;

        Vector3 startPos = transform.position;
        bounceSeq?.Kill();

        bounceSeq = DOTween.Sequence().SetAutoKill(true);

        // 1️⃣ 목표 위치에서 랜덤 offset 적용
        Vector2 randomOffset2D = Random.insideUnitCircle * randomRadius;
        Vector3 bounceCenter = targetPosition + new Vector3(randomOffset2D.x, 0f, randomOffset2D.y);

        // 2️⃣ 포물선 경로 설정
        Vector3 peakPos = Vector3.Lerp(startPos, bounceCenter, 0.5f) + Vector3.up * InitialBounceHeight * 5f;
        Vector3[] path = new Vector3[] { startPos, peakPos, bounceCenter };
        bounceSeq.Append(transform.DOPath(path, 1f, PathType.CatmullRom).SetEase(Ease.InQuad));

        // 3️⃣ 바운스 반복
        Vector3 currentPos = bounceCenter;
        for (int i = 0; i < BounceCount; i++)
        {
            float bouncePower = InitialBounceHeight * Mathf.Pow(0.5f, i + 1);

            Vector3 bouncePeak = currentPos + Vector3.up * bouncePower;

            bounceSeq.Append(transform.DOMove(bouncePeak, BounceDuration / 2f).SetEase(Ease.OutQuad));
            Vector3 floorPos = new Vector3(bounceCenter.x, targetPosition.y, bounceCenter.z);
            bounceSeq.Append(transform.DOMove(floorPos, BounceDuration / 2f).SetEase(Ease.InQuad));

            currentPos = floorPos;
        }

        // 4️⃣ 마지막에 _isAttractable 활성화
        bounceSeq.OnComplete(() => _isAttractable = true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsCollected) return;
        if (other.CompareTag("Player")) StartCoroutine(AttractToPlayer());
    }

    private IEnumerator AttractToPlayer()
    {
        while(_isAttractable == false)
        {
            yield return null;
        }

        DOTween.Kill(transform);

        IsCollected = true;

        Vector3 direction = (transform.position - _player.position).normalized;
        Vector3 retreatPos = transform.position + direction * RetreatDistance;

        Tween retreatTween = transform.DOMove(retreatPos, RetreatDuration).SetEase(Ease.OutQuad);
        yield return retreatTween.WaitForCompletion();

        // null 체크
        if (transform == null) yield break;

        Tween moveToPlayerTween = transform.DOMove(_player.position + Vector3.up * 0.3f, GetDuration).SetEase(Ease.InQuad);
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
