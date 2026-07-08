using UnityEngine;

/// <summary>
/// Returns a pooled effect to its pool after lifetime seconds.
/// </summary>
public class HiddenEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;

    void OnEnable()
    {
        Invoke(nameof(Hide), lifetime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    // Return to the pool immediately, before the lifetime expires
    // (e.g. a projectile that should vanish the moment it hits something).
    public void DespawnNow()
    {
        CancelInvoke(nameof(Hide));
        Hide();
    }

    void Hide() => PoolManager.Instance.Despawn(gameObject);
}
