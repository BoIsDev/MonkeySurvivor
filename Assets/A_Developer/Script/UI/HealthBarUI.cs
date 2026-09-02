using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Event-driven player health bar. Subscribes to Health.OnHealthChanged; no per-frame Update.
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text label;   // optional "current/max"

    private void OnEnable()  { if (playerHealth != null) playerHealth.OnHealthChanged += Refresh; }
    private void OnDisable() { if (playerHealth != null) playerHealth.OnHealthChanged -= Refresh; }
    private void Start()     { if (playerHealth != null) Refresh(playerHealth.Current, playerHealth.Max); }

    private void Refresh(float current, float max)
    {
        if (fill != null)  fill.fillAmount = max > 0f ? current / max : 0f;
        if (label != null) label.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
    }
}
