using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float invincibleDuration = 0f;

    public float Current { get; private set; }
    public float Max => maxHealth;

    public event Action OnDied;
    public event Action<float> OnDamageTaken;

    private bool isInvincible;

    private void OnEnable() => Current = maxHealth;

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;
        Current = Mathf.Max(0f, Current - amount);
        OnDamageTaken?.Invoke(amount);
        if (Current <= 0f) { OnDied?.Invoke(); return; }
        if (invincibleDuration > 0f) StartCoroutine(InvincibilityRoutine());
        Debug.Log("Debug HP " + Current);
    }

    // Blocks incoming damage for a set duration after being hit.
    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }
}
