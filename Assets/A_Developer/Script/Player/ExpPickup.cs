using UnityEngine;

/// <summary>
/// Dropped when an enemy dies; grants exp to the player on contact, then returns to the pool.
/// </summary>
public class ExpPickup : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private int value;

    public void Setup(int exp) => value = exp;

    private void OnTriggerEnter(Collider col)
    {
        if ((playerLayer.value & (1 << col.gameObject.layer)) == 0) return;
        col.GetComponentInParent<PlayerStats>()?.AddExp(value);
        Debug.Log(col.gameObject.name + ":" + value);
        PoolManager.Instance.Despawn(gameObject);
    }
}
