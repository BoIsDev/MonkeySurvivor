using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool continuous;
    [SerializeField] private float tickRate = 0.5f;
    // [SerializeField] private GameObject _prefab;

    private float nextTickTime;

    public void SetDamage(float value) => damage = value;

    private void OnTriggerEnter(Collider col)
    {
        if (continuous) return;
        col.GetComponent<IDamageable>()?.TakeDamage(damage);

        // if (_prefab != null)
        //     PoolManager.Instance.Despawn(_prefab, gameObject);
    }

    private void OnTriggerStay(Collider col)
    {
        if (!continuous) return;
        if (Time.time < nextTickTime) return;
        nextTickTime = Time.time + tickRate;
        col.GetComponent<IDamageable>()?.TakeDamage(damage);
    }
}
