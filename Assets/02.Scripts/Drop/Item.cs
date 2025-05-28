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

    private Transform _player;

    private void Start()
    {
        _player = PlayerManager.Instance.Player.transform;

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
            if (_tier <= i + 1) SparkleEffectList[i].SetActive(true);
            else SparkleEffectList[i].SetActive(false);
        }
        BounceEffect();
    }

    private void BounceEffect()
    {
        Vector3 startPos = transform.position;
        startPos = new Vector3(startPos.x, 1f, startPos.z);

        Vector3 randomDir = (Vector3.up + Random.onUnitSphere * 0.5f).normalized;
        Vector3 peakPos = startPos + randomDir * InitialBounceHeight;

        Sequence bounceSeq = DOTween.Sequence();

        for(int i = 0; i < BounceCount; i++)
        {
            float factor = 1f / (i + 1f);
            Vector3 upPeak = Vector3.Lerp(startPos, peakPos, factor);

            bounceSeq.Append(transform.DOMove(upPeak, BounceDuration / 2).SetEase(Ease.OutQuad));
            bounceSeq.Append(transform.DOMove(startPos, BounceDuration / 2).SetEase(Ease.InQuad));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsCollected) return;
        if (other.CompareTag("Player")) StartCoroutine(AttractToPlayer());
    }

    private IEnumerator AttractToPlayer()
    {
        IsCollected = true;

        Vector3 direction = (transform.position - _player.position).normalized;
        Vector3 retreatPos = transform.position + direction * RetreatDistance;

        yield return transform.DOMove(retreatPos, RetreatDuration).SetEase(Ease.OutQuad).WaitForCompletion();
        transform.DOScale(0.3f, GetDuration).SetEase(Ease.InOutQuad);
        yield return transform.DOMove(_player.position, GetDuration).SetEase(Ease.InQuad).WaitForCompletion();

        ApplyEffect();

        // 인벤토리에 넣기. DroppableItem이기 때문에 룬, 코인, 경험치가 될 수 있음

        switch (Type)
        {
            case EItemType.Rune:
                PlayerManager.Instance.PlayerSkill.AddRune(0, Rune);
                break;
            case EItemType.Exp:

                break;
            case EItemType.Coin:

                break;
        }

        Destroy(gameObject);
    }

    private void ApplyEffect()
    {
        // 먹을떄 이펙트
    }
}
