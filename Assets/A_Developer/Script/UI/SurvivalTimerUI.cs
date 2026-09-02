using UnityEngine;
using TMPro;

/// <summary>
/// Survival timer shown as mm:ss. Update + deltaTime for accuracy; the text is set only when the
/// whole-second value changes (so no per-frame TMP allocation).
/// </summary>
public class SurvivalTimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private float elapsed;
    private int lastShownSecond = -1;

    private void Update()
    {
        elapsed += Time.deltaTime;   // scaled → freezes while paused; sums to exact real elapsed time

        int totalSeconds = Mathf.FloorToInt(elapsed);
        if (totalSeconds == lastShownSecond) return;
        lastShownSecond = totalSeconds;

        if (timerText != null)
            timerText.text = $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";
    }
}
