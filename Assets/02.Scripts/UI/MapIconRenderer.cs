using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapIconRenderer : MonoBehaviour
{
    public RectTransform IconsContainer;
    public RectTransform PlayerIcon;
    public RectTransform EnemyIconPrefab;
    public Transform Player;

    private readonly Dictionary<Transform, RectTransform> _enemyIcons = new();

    void Update()
    {
        UpdateIcon(Player, PlayerIcon);

        var activeEnemies = EnemyTracker.GetActiveEnemies();

        foreach (Transform enemy in activeEnemies)
        {
            if (!_enemyIcons.ContainsKey(enemy))
            {
                RectTransform icon = Instantiate(EnemyIconPrefab, IconsContainer);
                _enemyIcons[enemy] = icon;
            }
            UpdateIcon(enemy, _enemyIcons[enemy]);
        }

        var toRemove = new List<Transform>();
        foreach (var kvp in _enemyIcons)
        {
            if (!activeEnemies.Contains(kvp.Key))
            {
                Destroy(kvp.Value.gameObject);
                toRemove.Add(kvp.Key);
            }
        }
        foreach (var tr in toRemove)
        {
            _enemyIcons.Remove(tr);
        }
    }

    void UpdateIcon(Transform target, RectTransform icon)
    {
        Vector3 pos = target.position;
        Vector2 min = Global.Instance.MapMin;
        Vector2 max = Global.Instance.MapMax;

        float u = Mathf.InverseLerp(min.x, max.x, pos.x);
        float v = Mathf.InverseLerp(min.y, max.y, pos.z);

        u = Mathf.Clamp01(u);
        v = Mathf.Clamp01(v);

        float width = IconsContainer.rect.width;
        float height = IconsContainer.rect.height;

        icon.anchoredPosition = new Vector2(
            (u - 0.5f) * width,
            (v - 0.5f) * height
        );
    }

}
