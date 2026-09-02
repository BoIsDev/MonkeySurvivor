using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Event-driven XP/level bar. Subscribes to PlayerStats exp/level events; no per-frame Update.
/// </summary>
public class XpBarUI : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text levelText;

    private void OnEnable()
    {
        if (stats == null) return;
        stats.OnExpChanged   += RefreshExp;
        stats.OnLevelChanged += RefreshLevel;
    }

    private void OnDisable()
    {
        if (stats == null) return;
        stats.OnExpChanged   -= RefreshExp;
        stats.OnLevelChanged -= RefreshLevel;
    }

    private void Start()
    {
        if (stats == null) return;
        RefreshExp(stats.CurrentExp, stats.ExpToNextLevel);
        RefreshLevel(stats.Level);
    }

    private void RefreshExp(int current, int need)
    {
        if (fill != null) fill.fillAmount = need > 0 ? (float)current / need : 0f;
    }

    private void RefreshLevel(int level)
    {
        if (levelText != null) levelText.text = $"Lv {level}";
    }
}
